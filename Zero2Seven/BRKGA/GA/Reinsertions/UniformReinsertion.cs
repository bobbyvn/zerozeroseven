using System.Collections.Generic;
using BRKGA.Exception;
using BRKGA.Interface;

namespace BRKGA.GA.Reinsertions
{
    public class UniformReinsertion<T> : ReinsertionBase<T>
    {
        public UniformReinsertion(IRandomization randomization) : base(false, true)
        {
            _randomization = randomization;
        }

        protected override IList<IChromosome<T>> PerformSelectChromosomes(IPopulation<T> population, IList<IChromosome<T>> offspring, IList<IChromosome<T>> parents)
        {
            if (offspring.Count == 0)
            {
                throw new ReinsertionException<T>(this, "The minimum size of the offspring is 1.");
            }

            while (offspring.Count < population.MinSize)
            {
                offspring.Add(offspring[_randomization.GetInt(0, offspring.Count)]);
            }

            return offspring;
        }

        private readonly IRandomization _randomization;
    }
}
