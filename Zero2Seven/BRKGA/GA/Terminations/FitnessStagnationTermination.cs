using System;
using BRKGA.Interface;

namespace BRKGA.GA.Terminations
{
    public class FitnessStagnationTermination<T> : TerminationBase<T>
    {
        public FitnessStagnationTermination() : this(100)
        {
        }

        public FitnessStagnationTermination(int expectedStagnantGenerationsNumber)
        {
            ExpectedStagnantGenerationsNumber = expectedStagnantGenerationsNumber;
        }

        public int ExpectedStagnantGenerationsNumber { get; set; }

        protected override bool PerformHasReached(IGeneticAlgorithm<T> geneticAlgorithm)
        {
            if (geneticAlgorithm.BestChromosome.Fitness != null)
            {
                var bestFitness = geneticAlgorithm.BestChromosome.Fitness.Value;

                if (Math.Abs(_lastFitness - bestFitness) < Double.Epsilon)
                {
                    _stagnantGenerationsCount++;
                }
                else
                {
                    _stagnantGenerationsCount = 1;
                }

                _lastFitness = bestFitness;
            }

            return _stagnantGenerationsCount >= ExpectedStagnantGenerationsNumber;
        }

        private double _lastFitness;
        private int _stagnantGenerationsCount;
    }
}
