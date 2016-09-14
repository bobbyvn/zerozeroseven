using System;
using System.Collections.Generic;
using BRKGA.Model;

namespace BRKGA.Interface
{
    public interface IPopulation<T>
    {
        event EventHandler BestChromosomeChanged;
        DateTime CreationDate { get; }
        IList<Generation<T>> Generations { get; }
        Generation<T> CurrentGeneration { get; }
        int GenerationsNumber { get; }
        int MinSize { get; set; }
        int MaxSize { get; set; }
        IChromosome<T> BestChromosome { get; }
        IGenerationStrategy<T> GenerationStrategy { get; set; }
        void CreateInitialGeneration();
        void CreateNewGeneration(IList<IChromosome<T>> chromosomes);
        void EndCurrentGeneration();
    }
}
