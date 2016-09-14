using System;
using BRKGA.Model;

namespace BRKGA.Interface
{
    public interface IChromosome<T> : IComparable<IChromosome<T>>
    {
        double? Fitness { get; set; }
        int Length { get; }
        T GenerateGene(int geneIndex);
        void ReplaceGene(int index, T gene);
        void ReplaceGenes(int startIndex, T[] genes);
        void Resize(int newLength);
        T GetGene(int index);
        T[] GetGenes();
        IChromosome<T> CreateNew();
        IChromosome<T> Clone();
    }
}
