using System.Collections.Generic;
using BRKGA.Interface;

namespace BRKGA.GA.Reinsertions
{
    public class PureReinsertion<T> : ReinsertionBase<T>
    {
        public PureReinsertion() : base(false, false)
        {
        }

        protected override IList<IChromosome<T>> PerformSelectChromosomes(IPopulation<T> population, IList<IChromosome<T>> offspring, IList<IChromosome<T>> parents)
        {
            return offspring;
        }
    }
}
