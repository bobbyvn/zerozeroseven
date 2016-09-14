using System;
using System.Collections.Generic;
using System.Linq;
using BRKGA.Interface;
using HelperSharp;

namespace BRKGA.Model
{
    public class Generation<T>
    {
        public Generation(int number, IList<IChromosome<T>> chromosomes)
        {
            if (number < 1)
            {
                throw new ArgumentOutOfRangeException(
                    "number",
                    "Generation number {0} is invalid. Generation number should be positive and start in 1.".With(number));
            }

            if (chromosomes == null || chromosomes.Count < 2)
            {
                throw new ArgumentOutOfRangeException("chromosomes", "A generation should have at least 2 chromosomes.");
            }

            Number = number;
            CreationDate = DateTime.Now;
            Chromosomes = chromosomes;
        }

        public int Number { get; private set; }
        public DateTime CreationDate { get; private set; }
        public IList<IChromosome<T>> Chromosomes { get; internal set; }
        public IChromosome<T> BestChromosome { get; internal set; }

        public void End(int chromosomesNumber)
        {
            Chromosomes = Chromosomes
                .Where(ValidateChromosome)
                .OrderByDescending(c => c.Fitness ?? 0)
                .ToList();

            if (Chromosomes.Count > chromosomesNumber)
            {
                Chromosomes = Chromosomes.Take(chromosomesNumber).ToList();
            }

            BestChromosome = Chromosomes.First();
        }

        private static bool ValidateChromosome(IChromosome<T> chromosome)
        {
            if (!chromosome.Fitness.HasValue)
            {
                throw new InvalidOperationException("There is unknown problem in current generation, because a chromosome has no fitness value.");
            }

            return true;
        }
    }
}
