using System.Collections.Generic;
using System.Linq;
using BRKGA.Interface;

namespace BRKGA.GA.Reinsertions
{
    public class FitnessBasedReinsertion<T> : ReinsertionBase<T>
    {
        public FitnessBasedReinsertion() : base(true, false)
        {
        }

        protected override IList<IChromosome<T>> PerformSelectChromosomes(IPopulation<T> population, IList<IChromosome<T>> offspring, IList<IChromosome<T>> parents)
        {
            if (offspring.Count > population.MaxSize)
            {
                return offspring.OrderByDescending(o => o.Fitness).Take(population.MaxSize).ToList();
            }

            return offspring;
        }
    }
}
