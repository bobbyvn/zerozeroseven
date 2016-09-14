using System.Collections.Generic;
using System.Linq;
using BRKGA.Interface;

namespace BRKGA.GA.Reinsertions
{
    public class ElitistReinsertion<T> : ReinsertionBase<T>
    {
        public ElitistReinsertion() : base(false, true)
        {
        }

        protected override IList<IChromosome<T>> PerformSelectChromosomes(IPopulation<T> population, IList<IChromosome<T>> offspring, IList<IChromosome<T>> parents)
        {
            var diff = population.MinSize - offspring.Count;

            if (diff > 0)
            {
                var bestParents = parents.OrderByDescending(p => p.Fitness).Take(diff);

                foreach (var p in bestParents)
                {
                    offspring.Add(p);
                }
            }

            return offspring;
        }
    }
}
