using System.Collections.Generic;
using BRKGA.Exception;
using BRKGA.Helper;
using BRKGA.Interface;
using System.Linq;

namespace BRKGA.GA.Crossovers
{
    public class PositionBasedCrossover<T> : OrderBasedCrossover<T>
    {
        public PositionBasedCrossover(IRandomization randomization) : base(randomization)
        {
        }

        protected override void ValidateParents(IList<IChromosome<T>> parents)
        {
            if (ChromosomeHelper<T>.AnyHasRepeatedGene(parents))
            {
                throw new CrossoverException<T>(this, "The Position-based Crossover (POS) can be only used with ordered chromosomes. The specified chromosome has repeated genes.");
            }
        }

        protected override IChromosome<T> CreateChild(IChromosome<T> firstParent, IChromosome<T> secondParent, int[] swapIndexes)
        {
            var firstParentGenes = new List<T>(firstParent.GetGenes());

            var child = firstParent.CreateNew();

            for (int i = 0; i < firstParent.Length; i++)
            {
                if (swapIndexes.Contains(i))
                {
                    var gene = secondParent.GetGene(i);
                    firstParentGenes.Remove(gene);
                    firstParentGenes.Insert(i, gene);
                }
            }

            child.ReplaceGenes(0, firstParentGenes.ToArray());

            return child;
        }
    }
}
