using System;
using System.Collections.Generic;
using System.Linq;
using BRKGA.Interface;
using HelperSharp;

namespace BRKGA.GA.Crossovers
{
    public class TwoPointCrossover<T> : OnePointCrossover<T>
    {
        public TwoPointCrossover(int swapPointOneGeneIndex, int swapPointTwoGeneIndex)
        {
            if (swapPointOneGeneIndex >= swapPointTwoGeneIndex)
            {
                throw new ArgumentOutOfRangeException("swapPointTwoGeneIndex", "The the swap point two index should be greater than swap point one index.");
            }

            SwapPointOneGeneIndex = swapPointOneGeneIndex;
            SwapPointTwoGeneIndex = swapPointTwoGeneIndex;
            MinChromosomeLength = 3;
        }

        public TwoPointCrossover() : this(0, 1)
        {
        }

        public int SwapPointOneGeneIndex { get; set; }

        public int SwapPointTwoGeneIndex { get; set; }

        protected override IList<IChromosome<T>> PerformCross(IList<IChromosome<T>> parents)
        {
            var firstParent = parents[0];
            var secondParent = parents[1];
            var parentLength = firstParent.Length;
            var swapPointsLength = parentLength - 1;

            if (SwapPointTwoGeneIndex >= swapPointsLength)
            {
                throw new ArgumentOutOfRangeException(
                    "parents",
                    "The swap point two index is {0}, but there is only {1} genes. The swap should result at least one gene to each sides.".With(SwapPointTwoGeneIndex, parentLength));
            }

            return CreateChildren(firstParent, secondParent);
        }

        protected override IChromosome<T> CreateChild(IChromosome<T> leftParent, IChromosome<T> rightParent)
        {
            var firstCutGenesCount = SwapPointOneGeneIndex + 1;
            var secondCutGenesCount = SwapPointTwoGeneIndex + 1;
            var child = leftParent.CreateNew();
            child.ReplaceGenes(0, leftParent.GetGenes().Take(firstCutGenesCount).ToArray());
            child.ReplaceGenes(firstCutGenesCount, rightParent.GetGenes().Skip(firstCutGenesCount).Take(secondCutGenesCount - firstCutGenesCount).ToArray());
            child.ReplaceGenes(secondCutGenesCount, leftParent.GetGenes().Skip(secondCutGenesCount).ToArray());

            return child;
        }
    }
}
