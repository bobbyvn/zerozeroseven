using System;

namespace BRKGA.Interface
{
    public interface IGeneticAlgorithm<T>
    {
        int GenerationsNumber { get; }
        IChromosome<T> BestChromosome { get; }
        TimeSpan TimeEvolving { get; }
    }
}
