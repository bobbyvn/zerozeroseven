using System.Collections.Generic;
using BRKGA.Interface;
using BRKGA.Model;

namespace BRKGA.GA.Selections
{
    public class StochasticUniversalSamplingSelection<T> : RouletteWheelSelection<T>
    {
        public StochasticUniversalSamplingSelection(IRandomization randomization) : base(randomization)
        {
        }

        protected override IList<IChromosome<T>> PerformSelectChromosomes(int number, Generation<T> generation)
        {
            var chromosomes = generation.Chromosomes;
            var rouleteWheel = new List<double>();
            double stepSize = 1.0 / number;

            CalculateCumulativePercentFitness(chromosomes, rouleteWheel);

            var pointer = _randomization.GetDouble();

            return SelectFromWheel(
                number,
                chromosomes,
                rouleteWheel,
                () =>
                {
                    if (pointer > 1.0)
                    {
                        pointer -= 1.0;
                    }

                    var currentPointer = pointer;
                    pointer += stepSize;

                    return currentPointer;
                });
        }
    }
}
