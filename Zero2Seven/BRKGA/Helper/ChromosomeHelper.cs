using System;
using System.Collections.Generic;
using System.Linq;
using BRKGA.Interface;
using HelperSharp;

namespace BRKGA.Helper
{
    public class ChromosomeHelper<T>
    {
        public static bool AnyHasRepeatedGene(IList<IChromosome<T>> chromosomes)
        {
            foreach (var p in chromosomes)
            {
                var notRepeatedGenesLength = p.GetGenes().Distinct().Count();

                if (notRepeatedGenesLength < p.Length)
                {
                    return true;
                }
            }

            return false;
        }

        public static void ValidateGenes(IList<IChromosome<T>> chromosomes)
        {
            if (chromosomes.Any(c => c.GetGenes().Any(g => g == null)))
            {
                throw new InvalidOperationException("The chromosome '{0}' is generating genes with null value.".With(chromosomes.First().GetType().Name));
            }
        }

        public static void ValidateGenes(IChromosome<T> chromosome)
        {
            if (chromosome != null && chromosome.GetGenes().Any(g => g == null))
            {
                throw new InvalidOperationException("The chromosome '{0}' is generating genes with null value.".With(chromosome.GetType().Name));
            }
        }
    }
}
