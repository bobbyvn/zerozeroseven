using System;
using System.Collections.Generic;
using System.Linq;
using BRKGA.Exception;
using BRKGA.Helper;
using BRKGA.Interface;

namespace BRKGA.GA.Crossovers
{
    public sealed class OrderedCrossover<T> : CrossoverBase<T>
    {
        public OrderedCrossover(IRandomization randomization)
            : base(2, 2)
        {
            IsOrdered = true;
            _randomization = randomization;
        }

        protected override IList<IChromosome<T>> PerformCross(IList<IChromosome<T>> parents)
        {
            var firstParent = parents[0];
            var secondParent = parents[1];

            if (ChromosomeHelper<T>.AnyHasRepeatedGene(parents))
            {
                throw new CrossoverException<T>(this, "The Ordered Crossover (OX1) can be only used with ordered chromosomes. The specified chromosome has repeated genes.");
            }

            var middleSectionIndexes = _randomization.GetUniqueInts(2, 0, firstParent.Length);
            Array.Sort(middleSectionIndexes);
            var middleSectionBeginIndex = middleSectionIndexes[0];
            var middleSectionEndIndex = middleSectionIndexes[1];
            var firstChild = CreateChild(firstParent, secondParent, middleSectionBeginIndex, middleSectionEndIndex);
            var secondChild = CreateChild(secondParent, firstParent, middleSectionBeginIndex, middleSectionEndIndex);

            return new List<IChromosome<T>>() { firstChild, secondChild };
        }

        private static IChromosome<T> CreateChild(IChromosome<T> firstParent, IChromosome<T> secondParent, int middleSectionBeginIndex, int middleSectionEndIndex)
        {
            var middleSectionGenes = firstParent.GetGenes().Skip(middleSectionBeginIndex).Take((middleSectionEndIndex - middleSectionBeginIndex) + 1);
            var secondParentRemainingGenes = secondParent.GetGenes().Except(middleSectionGenes).GetEnumerator();
            var child = firstParent.CreateNew();

            for (int i = 0; i < firstParent.Length; i++)
            {
                var firstParentGene = firstParent.GetGene(i);

                if (i >= middleSectionBeginIndex && i <= middleSectionEndIndex)
                {
                    child.ReplaceGene(i, firstParentGene);
                }
                else
                {
                    secondParentRemainingGenes.MoveNext();
                    child.ReplaceGene(i, secondParentRemainingGenes.Current);
                }
            }

            return child;
        }

        private readonly IRandomization _randomization;
    }
}
