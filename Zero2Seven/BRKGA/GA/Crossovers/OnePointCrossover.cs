using System;
using System.Collections.Generic;
using System.Linq;
using BRKGA.Interface;
using HelperSharp;

namespace BRKGA.GA.Crossovers
{
    public class OnePointCrossover<T> : CrossoverBase<T>
    {
        public OnePointCrossover(int swapPointIndex) : base(2, 2)
        {
            SwapPointIndex = swapPointIndex;
        }

        public OnePointCrossover() : this(0)
        {
        }

        public int SwapPointIndex { get; set; }

        protected override IList<IChromosome<T>> PerformCross(IList<IChromosome<T>> parents)
        {
            var firstParent = parents[0];
            var secondParent = parents[1];

            var swapPointsLength = firstParent.Length - 1;

            if (SwapPointIndex >= swapPointsLength)
            {
                throw new ArgumentOutOfRangeException(
                    "parents",
                    "The swap point index is {0}, but there is only {1} genes. The swap should result at least one gene to each side.".With(SwapPointIndex, firstParent.Length));
            }

            return CreateChildren(firstParent, secondParent);
        }

        protected IList<IChromosome<T>> CreateChildren(IChromosome<T> firstParent, IChromosome<T> secondParent)
        {
            var firstChild = CreateChild(firstParent, secondParent);
            var secondChild = CreateChild(secondParent, firstParent);

            return new List<IChromosome<T>>() { firstChild, secondChild };
        }

        protected virtual IChromosome<T> CreateChild(IChromosome<T> leftParent, IChromosome<T> rightParent)
        {
            var cutGenesCount = SwapPointIndex + 1;
            var child = leftParent.CreateNew();
            child.ReplaceGenes(0, leftParent.GetGenes().Take(cutGenesCount).ToArray());
            child.ReplaceGenes(cutGenesCount, rightParent.GetGenes().Skip(cutGenesCount).ToArray());

            return child;
        }
    }
}
