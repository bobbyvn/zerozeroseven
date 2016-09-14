using System.Collections.Generic;
using BRKGA.Interface;

namespace BRKGA.GA.Crossovers
{
    public class ThreeParentCrossover<T> : CrossoverBase<T>
    {
        public ThreeParentCrossover()
            : base(3, 1)
        {
        }

        protected override IList<IChromosome<T>> PerformCross(IList<IChromosome<T>> parents)
        {
            var parent1 = parents[0];
            var parent1Genes = parent1.GetGenes();
            var parent2Genes = parents[1].GetGenes();
            var parent3Genes = parents[2].GetGenes();
            var offspring = parent1.CreateNew();

            for (int i = 0; i < parent1.Length; i++)
            {
                var parent1Gene = parent1Genes[i];

                offspring.ReplaceGene(i, parent1Gene.Equals(parent2Genes[i]) ? parent1Gene : parent3Genes[i]);
            }

            return new List<IChromosome<T>>() { offspring };
        }
    }
}
