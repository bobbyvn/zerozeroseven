using System.Collections.Generic;
using System.Linq;
using BRKGA.Exception;
using BRKGA.Helper;
using BRKGA.Interface;

namespace BRKGA.GA.Crossovers
{
    public class PartiallyMappedCrossover<T> : CrossoverBase<T>
    {
        public PartiallyMappedCrossover(IRandomization randomization) : base(2, 2, 3)   
        {
            IsOrdered = true;
            _randomization = randomization;
        }

        protected override IList<IChromosome<T>> PerformCross(IList<IChromosome<T>> parents)
        {
            if (ChromosomeHelper<T>.AnyHasRepeatedGene(parents))
            {
                throw new CrossoverException<T>(this, "The Partially Mapped Crossover (PMX) can be only used with ordered chromosomes. The specified chromosome has repeated genes.");
            }

            var parent1 = parents[0];
            var parent2 = parents[1];

            var cutPointsIndexes = _randomization.GetUniqueInts(2, 0, parent1.Length);
            var firstCutPointIndex = cutPointsIndexes[0];
            var secondCutPointIdnex = cutPointsIndexes[1];

            var parent1Genes = parent1.GetGenes();
            var parent1MappingSection = parent1Genes.Skip(firstCutPointIndex).Take((secondCutPointIdnex - firstCutPointIndex) + 1).ToArray();

            var parent2Genes = parent2.GetGenes();
            var parent2MappingSection = parent2Genes.Skip(firstCutPointIndex).Take((secondCutPointIdnex - firstCutPointIndex) + 1).ToArray();

            var offspring1 = parent1.CreateNew();
            var offspring2 = parent2.CreateNew();

            offspring2.ReplaceGenes(firstCutPointIndex, parent1MappingSection);
            offspring1.ReplaceGenes(firstCutPointIndex, parent2MappingSection);

            var length = parent1.Length;

            for (int i = 0; i < length; i++)
            {
                if (i >= firstCutPointIndex && i <= secondCutPointIdnex)
                {
                    continue;
                }

                var geneForoffspring1 = GetGeneNotInMappingSection(parent1Genes[i], parent2MappingSection, parent1MappingSection);
                offspring1.ReplaceGene(i, geneForoffspring1);

                var geneForoffspring2 = GetGeneNotInMappingSection(parent2Genes[i], parent1MappingSection, parent2MappingSection);
                offspring2.ReplaceGene(i, geneForoffspring2);
            }

            return new List<IChromosome<T>>() { offspring1, offspring2 };
        }

        private T GetGeneNotInMappingSection(T candidateGene, T[] mappingSection, T[] otherParentMappingSection)
        {
            var indexOnMappingSection = mappingSection
                .Select((item, index) => new { Gene = item, Index = index })
                .FirstOrDefault(g => g.Gene.Equals(candidateGene));

            if (indexOnMappingSection != null)
            {
                return GetGeneNotInMappingSection(otherParentMappingSection[indexOnMappingSection.Index], mappingSection, otherParentMappingSection);
            }

            return candidateGene;
        }

        private readonly IRandomization _randomization;
    }
}
