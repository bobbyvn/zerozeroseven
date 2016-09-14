using System.Collections.Generic;
using System.Linq;
using BRKGA.Interface;
using BRKGA.Model;

namespace BRKGA.GA.Selections
{
    public sealed class EliteSelection<T> : SelectionBase<T>
    {
        public EliteSelection() : base(2)
        {
        }

        protected override IList<IChromosome<T>> PerformSelectChromosomes(int number, Generation<T> generation)
        {
            var ordered = generation.Chromosomes.OrderByDescending(c => c.Fitness);
            return ordered.Take(number).ToList();
        }
    }
}
