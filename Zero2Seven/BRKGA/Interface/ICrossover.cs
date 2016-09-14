using System.Collections.Generic;

namespace BRKGA.Interface
{
    public interface ICrossover<T> : IChromosomeOperator
    {
        int ParentsNumber { get; }
        int ChildrenNumber { get; }
        IList<IChromosome<T>> Cross(IList<IChromosome<T>> parents);
    }
}
