using BRKGA.Interface;

namespace BRKGA.GA.Terminations
{
    public class GenerationNumberTermination<T> : TerminationBase<T>
    {
        public GenerationNumberTermination() : this(100)
        {
        }

        public GenerationNumberTermination(int expectedGenerationNumber)
        {
            ExpectedGenerationNumber = expectedGenerationNumber;
        }

        public int ExpectedGenerationNumber { get; set; }

        protected override bool PerformHasReached(IGeneticAlgorithm<T> geneticAlgorithm)
        {
            return geneticAlgorithm.GenerationsNumber >= ExpectedGenerationNumber;
        }
    }
}
