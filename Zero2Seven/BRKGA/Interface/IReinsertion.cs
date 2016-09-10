using System.Collections.Generic;

namespace BRKGA.Interface
{
    public interface IReinsertion<T>
    {
        bool CanCollapse { get; }
        bool CanExpand { get; }
        IList<IChromosome<T>> SelectChromosomes(IPopulation<T> population, IList<IChromosome<T>> offspring, IList<IChromosome<T>> parents);
    }
}
