using BRKGA.Interface;
using HelperSharp;

namespace BRKGA.GA
{
    public class PerformanceGenerationStrategy<T> : IGenerationStrategy<T>
    {
        public PerformanceGenerationStrategy()
        {
            GenerationsNumber = 1;
        }

        public PerformanceGenerationStrategy(int generationsNumber)
        {
            GenerationsNumber = generationsNumber;
        }
        
        public int GenerationsNumber { get; set; }

        public void RegisterNewGeneration(IPopulation<T> population)
        {
            ExceptionHelper.ThrowIfNull("population", population);

            if (population.Generations.Count > GenerationsNumber)
            {
                population.Generations.RemoveAt(0);
            }
        }
    }
}
