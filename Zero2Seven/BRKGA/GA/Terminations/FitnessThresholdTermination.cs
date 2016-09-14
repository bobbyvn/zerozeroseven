using BRKGA.Interface;

namespace BRKGA.GA.Terminations
{
    public class FitnessThresholdTermination<T> : TerminationBase<T>
    {
        public FitnessThresholdTermination() : this(1.00)
        {
        }

        public FitnessThresholdTermination(double expectedFitness)
        {
            ExpectedFitness = expectedFitness;
        }

        public double ExpectedFitness { get; set; }

        protected override bool PerformHasReached(IGeneticAlgorithm<T> geneticAlgorithm)
        {
            return geneticAlgorithm.BestChromosome.Fitness >= ExpectedFitness;
        }
    }
}
