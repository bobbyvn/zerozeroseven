using System.Collections.Generic;
using System.Linq;
using BRKGA.Exception;
using BRKGA.Helper;
using BRKGA.Interface;

namespace BRKGA.GA.Crossovers
{
    public class OrderBasedCrossover<T> : CrossoverBase<T>
    {
        public OrderBasedCrossover(IRandomization randomization)
            : base(2, 2)
        {
            IsOrdered = true;
            _randomization = randomization;
        }

        protected override IList<IChromosome<T>> PerformCross(IList<IChromosome<T>> parents)
        {
            ValidateParents(parents);

            var firstParent = parents[0];
            var secondParent = parents[1];

            var swapIndexesLength = _randomization.GetInt(1, firstParent.Length - 1);
            var swapIndexes = _randomization.GetUniqueInts(swapIndexesLength, 0, firstParent.Length);
            var firstChild = CreateChild(firstParent, secondParent, swapIndexes);
            var secondChild = CreateChild(secondParent, firstParent, swapIndexes);

            return new List<IChromosome<T>>() { firstChild, secondChild };
        }

        protected virtual void ValidateParents(IList<IChromosome<T>> parents)
        {
            if (ChromosomeHelper<T>.AnyHasRepeatedGene(parents))
            {
                throw new CrossoverException<T>(this, "The Order-based Crossover (OX2) can be only used with ordered chromosomes. The specified chromosome has repeated genes.");
            }
        }

        protected virtual IChromosome<T> CreateChild(IChromosome<T> firstParent, IChromosome<T> secondParent, int[] swapIndexes)
        {
            // ...suppose that in the second parent in the second, third 
            // and sixth positions are selected. The elements in these positions are 4, 6 and 5 respectively...
            var secondParentSwapGenes = secondParent.GetGenes()
                .Select((g, i) => new { Gene = g, Index = i })
                .Where((g) => swapIndexes.Contains(g.Index))
                .ToArray();

            var firstParentGenes = firstParent.GetGenes();

            // ...in the first parent, these elements are present at the fourth, fifth and sixth positions...
            var firstParentSwapGenes = firstParentGenes
                .Select((g, i) => new { Gene = g, Index = i })
                .Where((g) => secondParentSwapGenes.Any(s => s.Gene.Equals(g.Gene)))
                .ToArray();

            var child = firstParent.CreateNew();
            var secondParentSwapGensIndex = 0;

            for (int i = 0; i < firstParent.Length; i++)
            {
                // Now the offspring are equal to parent 1 except in the fourth, fifth and sixth positions.
                // We add the missing elements to the offspring in the same order in which they appear in the second parent.                
                child.ReplaceGene(i, firstParentSwapGenes.Any(f => f.Index == i)
                        ? secondParentSwapGenes[secondParentSwapGensIndex++].Gene
                        : firstParentGenes[i]);
            }

            return child;
        }

        private readonly IRandomization _randomization;
    }
}
