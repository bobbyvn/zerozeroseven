using System.Collections.Generic;
using System.Linq;
using BRKGA.Exception;
using BRKGA.Helper;
using BRKGA.Interface;

namespace BRKGA.GA.Crossovers
{
    public class CycleCrossover<T> : CrossoverBase<T>
    {
        public CycleCrossover()
            : base(2, 2)
        {
            IsOrdered = true;
        }

        protected override IList<IChromosome<T>> PerformCross(IList<IChromosome<T>> parents)
        {
            var parent1 = parents[0];
            var parent2 = parents[1];

            if (ChromosomeHelper<T>.AnyHasRepeatedGene(parents))
            {
                throw new CrossoverException<T>(this, "The Cycle Crossover (CX) can be only used with ordered chromosomes. The specified chromosome has repeated genes.");
            }

            var cycles = new List<List<int>>();
            var offspring1 = parent1.CreateNew();
            var offspring2 = parent2.CreateNew();

            var parent1Genes = parent1.GetGenes();
            var parent2Genes = parent2.GetGenes();

            // Search for the cycles.
            for (int i = 0; i < parent1.Length; i++)
            {
                if (!cycles.SelectMany(p => p).Contains(i))
                {
                    var cycle = new List<int>();
                    CreateCycle(parent1Genes, parent2Genes, i, cycle);
                    cycles.Add(cycle);
                }
            }

            // Copy the cycles to the offpring.
            for (int i = 0; i < cycles.Count; i++)
            {
                var cycle = cycles[i];

                if (i % 2 == 0)
                {
                    // Copy cycle index pair: values from Parent 1 and copied to Child 1, and values from Parent 2 will be copied to Child 2.
                    CopyCycleIndexPair(cycle, parent1Genes, offspring1, parent2Genes, offspring2);
                }
                else
                {
                    // Copy cycle index odd: values from Parent 1 will be copied to Child 2, and values from Parent 1 will be copied to Child 1.
                    CopyCycleIndexPair(cycle, parent1Genes, offspring2, parent2Genes, offspring1);
                }
            }

            return new List<IChromosome<T>>() { offspring1, offspring2 };
        }

        private static void CopyCycleIndexPair(IList<int> cycle, T[] fromParent1Genes, IChromosome<T> toOffspring1, T[] fromParent2Genes, IChromosome<T> toOffspring2)
        {
            foreach (var geneCycleIndex in cycle)
            {
                toOffspring1.ReplaceGene(geneCycleIndex, fromParent1Genes[geneCycleIndex]);
                toOffspring2.ReplaceGene(geneCycleIndex, fromParent2Genes[geneCycleIndex]);
            }
        }

        private void CreateCycle(T[] parent1Genes, T[] parent2Genes, int geneIndex, List<int> cycle)
        {
            if (!cycle.Contains(geneIndex))
            {
                var parent2Gene = parent2Genes[geneIndex];
                cycle.Add(geneIndex);
                var newGeneIndex = parent1Genes.Select((g, i) => new { Value = g, Index = i }).First(g => g.Value.Equals(parent2Gene));

                if (geneIndex != newGeneIndex.Index)
                {
                    CreateCycle(parent1Genes, parent2Genes, newGeneIndex.Index, cycle);
                }
            }
        }
    }
}
