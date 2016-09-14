using System;
using System.Collections.Generic;
using BRKGA.Helper;
using BRKGA.Interface;
using BRKGA.Model;
using HelperSharp;

namespace BRKGA.GA
{
    public class Population<T> : IPopulation<T>
    {
        public Population(int minSize, int maxSize, IChromosome<T> adamChromosome)
        {
            if (minSize < 2)
            {
                throw new ArgumentOutOfRangeException("minSize", "The minimum size for a population is 2 chromosomes.");
            }

            if (maxSize < minSize)
            {
                throw new ArgumentOutOfRangeException("maxSize", "The maximum size for a population should be equal or greater than minimum size.");
            }

            ExceptionHelper.ThrowIfNull("adamChromosome", adamChromosome);

            CreationDate = DateTime.Now;
            MinSize = minSize;
            MaxSize = maxSize;
            AdamChromosome = adamChromosome;
            Generations = new List<Generation<T>>();
            GenerationStrategy = new PerformanceGenerationStrategy<T>(10);
        }

        public event EventHandler BestChromosomeChanged;

        public DateTime CreationDate { get; protected set; }

        public IList<Generation<T>> Generations { get; protected set; }
        public Generation<T> CurrentGeneration { get; protected set; }
        public int GenerationsNumber { get; protected set; }
        public int MinSize { get; set; }
        public int MaxSize { get; set; }
        public IChromosome<T> BestChromosome { get; protected set; }
        public IGenerationStrategy<T> GenerationStrategy { get; set; }
        protected IChromosome<T> AdamChromosome { get; set; }

        public virtual void CreateInitialGeneration()
        {
            Generations = new List<Generation<T>>();
            GenerationsNumber = 0;

            var chromosomes = new List<IChromosome<T>>();

            for (int i = 0; i < MinSize; i++)
            {
                var c = AdamChromosome.CreateNew();

                if (c == null)
                {
                    throw new InvalidOperationException("The Adam chromosome's 'CreateNew' method generated a null chromosome. This is a invalid behavior, please, check your chromosome code.");
                }

                ChromosomeHelper<T>.ValidateGenes(c);

                chromosomes.Add(c);
            }

            CreateNewGeneration(chromosomes);
        }

        public virtual void CreateNewGeneration(IList<IChromosome<T>> chromosomes)
        {
            ExceptionHelper.ThrowIfNull("chromosomes", chromosomes);
            ChromosomeHelper<T>.ValidateGenes(chromosomes);

            CurrentGeneration = new Generation<T>(++GenerationsNumber, chromosomes);
            Generations.Add(CurrentGeneration);
            GenerationStrategy.RegisterNewGeneration(this);
        }

        public virtual void EndCurrentGeneration()
        {
            CurrentGeneration.End(MaxSize);

            if (BestChromosome != CurrentGeneration.BestChromosome)
            {
                BestChromosome = CurrentGeneration.BestChromosome;

                OnBestChromosomeChanged(EventArgs.Empty);
            }
        }

        protected virtual void OnBestChromosomeChanged(EventArgs args)
        {
            if (BestChromosomeChanged != null)
            {
                BestChromosomeChanged(this, args);
            }
        }
    }
}
