using System;
using System.Collections.Generic;
using System.Linq;
using BRKGA.Exception;
using BRKGA.Interface;
using BRKGA.Model.Enum;
using HelperSharp;

namespace BRKGA.GA
{
    public sealed class GeneticAlgorithmBase<T> : IGeneticAlgorithm<T>
    {
        public GeneticAlgorithmBase(
                          IPopulation<T> population,
                          IFitness<T> fitness,
                          ISelection<T> selection,
                          ICrossover<T> crossover,
                          IMutation<T> mutation,
                          IReinsertion<T> reinsertion,
                          ITermination<T> termination,
                          IRandomization randomization,
                          ITaskExecutor taskExecutor, float crossoverProbability, float mutationProbability)
        {
            ExceptionHelper.ThrowIfNull("Population", population);
            ExceptionHelper.ThrowIfNull("fitness", fitness);
            ExceptionHelper.ThrowIfNull("selection", selection);
            ExceptionHelper.ThrowIfNull("crossover", crossover);
            ExceptionHelper.ThrowIfNull("mutation", mutation);
            ExceptionHelper.ThrowIfNull("termination", termination);
            ExceptionHelper.ThrowIfNull("randomization", randomization);
            ExceptionHelper.ThrowIfNull("taskExecutor", taskExecutor);

            Population = population;
            Fitness = fitness;
            Selection = selection;
            Crossover = crossover;
            Mutation = mutation;
            Reinsertion = reinsertion;
            Termination = termination;
            Randomization = randomization;
            TaskExecutor = taskExecutor;

            CrossoverProbability = crossoverProbability;
            MutationProbability = mutationProbability;
            TimeEvolving = TimeSpan.Zero;
            State = GeneticAlgorithmState.NotStarted;
        }

        public event EventHandler GenerationRan;
        public event EventHandler TerminationReached;
        public event EventHandler Stopped;

        public IPopulation<T> Population { get; private set; }
        public IFitness<T> Fitness { get; private set; }
        public ISelection<T> Selection { get; set; }
        public ICrossover<T> Crossover { get; set; }
        public float CrossoverProbability { get; set; }
        public IMutation<T> Mutation { get; set; }
        public float MutationProbability { get; set; }
        public IReinsertion<T> Reinsertion { get; set; }
        public ITermination<T> Termination { get; set; }
        public IRandomization Randomization { get; set; }
        public int GenerationsNumber
        {
            get
            {
                return Population.GenerationsNumber;
            }
        }

        public IChromosome<T> BestChromosome
        {
            get
            {
                return Population.BestChromosome;
            }
        }

        public TimeSpan TimeEvolving { get; private set; }

        public GeneticAlgorithmState State
        {
            get
            {
                return _state;
            }

            private set
            {
                var shouldStop = Stopped != null && _state != value && value == GeneticAlgorithmState.Stopped;

                _state = value;

                if (shouldStop)
                {
                    Stopped(this, EventArgs.Empty);
                }
            }
        }

        public bool IsRunning
        {
            get
            {
                return State == GeneticAlgorithmState.Started || State == GeneticAlgorithmState.Resumed;
            }
        }

        public ITaskExecutor TaskExecutor { get; set; }

        public void Start()
        {
            lock (_lock)
            {
                State = GeneticAlgorithmState.Started;
                var startDateTime = DateTime.Now;
                Population.CreateInitialGeneration();
                TimeEvolving = DateTime.Now - startDateTime;
            }

            Resume();
        }

        public void Resume()
        {
            try
            {
                lock (_lock)
                {
                    _stopRequested = false;
                }

                if (Population.GenerationsNumber == 0)
                {
                    throw new InvalidOperationException("Attempt to resume a genetic algorithm which was not yet started.");
                }
                else if (Population.GenerationsNumber > 1)
                {
                    if (Termination.HasReached(this))
                    {
                        throw new InvalidOperationException("Attempt to resume a genetic algorithm with a termination ({0}) already reached. Please, specify a new termination or extend the current one.".With(Termination));
                    }

                    State = GeneticAlgorithmState.Resumed;
                }

                if (EndCurrentGeneration())
                {
                    return;
                }

                bool terminationConditionReached = false;
                DateTime startDateTime;

                do
                {
                    if (_stopRequested)
                    {
                        break;
                    }

                    startDateTime = DateTime.Now;
                    terminationConditionReached = EvolveOneGeneration();
                    TimeEvolving += DateTime.Now - startDateTime;
                }
                while (!terminationConditionReached);
            }
            catch
            {
                State = GeneticAlgorithmState.Stopped;
                throw;
            }
        }

        public void Stop()
        {
            if (Population.GenerationsNumber == 0)
            {
                throw new InvalidOperationException("Attempt to stop a genetic algorithm which was not yet started.");
            }

            lock (_lock)
            {
                _stopRequested = true;
            }
        }

        private bool EvolveOneGeneration()
        {
            var parents = SelectParents();
            var offspring = Cross(parents);
            Mutate(offspring);
            var newGenerationChromosomes = Reinsert(offspring, parents);
            Population.CreateNewGeneration(newGenerationChromosomes);
            return EndCurrentGeneration();
        }

        private bool EndCurrentGeneration()
        {
            EvaluateFitness();
            Population.EndCurrentGeneration();

            if (GenerationRan != null)
            {
                GenerationRan(this, EventArgs.Empty);
            }

            if (Termination.HasReached(this))
            {
                State = GeneticAlgorithmState.TerminationReached;

                if (TerminationReached != null)
                {
                    TerminationReached(this, EventArgs.Empty);
                }

                return true;
            }

            if (_stopRequested)
            {
                TaskExecutor.Stop();
                State = GeneticAlgorithmState.Stopped;
            }

            return false;
        }

        private void EvaluateFitness()
        {
            try
            {
                var chromosomesWithoutFitness = Population.CurrentGeneration.Chromosomes.Where(c => !c.Fitness.HasValue).ToList();

                for (int i = 0; i < chromosomesWithoutFitness.Count; i++)
                {
                    var c = chromosomesWithoutFitness[i];

                    TaskExecutor.Add(() =>
                    {
                        RunEvaluateFitness(c);
                    });
                }

                if (!TaskExecutor.Start())
                {
                    throw new TimeoutException("The fitness evaluation rech the {0} timeout.".With(TaskExecutor.Timeout));
                }
            }
            finally
            {
                TaskExecutor.Stop();
                TaskExecutor.Clear();
            }

            Population.CurrentGeneration.Chromosomes = Population.CurrentGeneration.Chromosomes.OrderByDescending(c => c.Fitness.Value).ToList();
        }

        private object RunEvaluateFitness(object chromosome)
        {
            var c = chromosome as IChromosome<T>;

            try
            {
                c.Fitness = Fitness.Evaluate(c);
            }
            catch (System.Exception ex)
            {
                throw new FitnessException<T>(Fitness, "Error executing Fitness.Evaluate for chromosome: {0}".With(ex.Message), ex);
            }

            return c.Fitness;
        }

        private IList<IChromosome<T>> SelectParents()
        {
            return Selection.SelectChromosomes(Population.MinSize, Population.CurrentGeneration);
        }

        private IList<IChromosome<T>> Cross(IList<IChromosome<T>> parents)
        {
            var offspring = new List<IChromosome<T>>();

            for (int i = 0; i < Population.MinSize; i += Crossover.ParentsNumber)
            {
                var selectedParents = parents.Skip(i).Take(Crossover.ParentsNumber).ToList();

                // If match the probability cross is made, otherwise the offspring is an exact copy of the parents.
                // Checks if the number of selected parents is equal which the crossover expect, because the in the end of the list we can
                // have some rest chromosomes.
                if (selectedParents.Count == Crossover.ParentsNumber && Randomization.GetDouble() <= CrossoverProbability)
                {
                    offspring.AddRange(Crossover.Cross(selectedParents));
                }
            }

            return offspring;
        }

        private void Mutate(IList<IChromosome<T>> chromosomes)
        {
            foreach (var c in chromosomes)
            {
                Mutation.Mutate(c, MutationProbability);
            }
        }

        private IList<IChromosome<T>> Reinsert(IList<IChromosome<T>> offspring, IList<IChromosome<T>> parents)
        {
            return Reinsertion.SelectChromosomes(Population, offspring, parents);
        }

        private bool _stopRequested;
        private object _lock = new object();
        private GeneticAlgorithmState _state;
    }
}
