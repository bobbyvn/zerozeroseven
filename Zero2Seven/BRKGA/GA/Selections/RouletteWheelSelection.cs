using System;
using System.Collections.Generic;
using System.Linq;
using BRKGA.Interface;
using BRKGA.Model;

namespace BRKGA.GA.Selections
{
    public class RouletteWheelSelection<T> : SelectionBase<T>
    {
        public RouletteWheelSelection(IRandomization randomization) : base(2)
        {
            _randomization = randomization;
        }

        protected static IList<IChromosome<T>> SelectFromWheel(int number, IList<IChromosome<T>> chromosomes, IList<double> rouletteWheel, Func<double> getPointer)
        {
            var selected = new List<IChromosome<T>>();

            for (int i = 0; i < number; i++)
            {
                var pointer = getPointer();

                var selection = rouletteWheel.Select((value, index) => new { Value = value, Index = index }).FirstOrDefault(r => r.Value >= pointer);
                if (selection != null)
                {
                    var chromosomeIndex = selection.Index;
                    selected.Add(chromosomes[chromosomeIndex]);
                }
            }

            return selected;
        }

        protected static void CalculateCumulativePercentFitness(IList<IChromosome<T>> chromosomes, IList<double> rouletteWheel)
        {
            var sumFitness = chromosomes.Sum(c => c.Fitness != null ? c.Fitness.Value : 0);

            var cumulativePercent = 0.0;

            foreach (var c in chromosomes)
            {
                if (c.Fitness != null) cumulativePercent += c.Fitness.Value / sumFitness;
                rouletteWheel.Add(cumulativePercent);
            }
        }

        protected override IList<IChromosome<T>> PerformSelectChromosomes(int number, Generation<T> generation)
        {
            var chromosomes = generation.Chromosomes;
            var rouletteWheel = new List<double>();
            CalculateCumulativePercentFitness(chromosomes, rouletteWheel);

            return SelectFromWheel(number, chromosomes, rouletteWheel, () => _randomization.GetDouble());
        }

        protected readonly IRandomization _randomization;
    }
}
