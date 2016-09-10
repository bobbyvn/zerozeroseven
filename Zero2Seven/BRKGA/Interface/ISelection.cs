using System.Collections.Generic;
using BRKGA.Model;

namespace BRKGA.Interface
{
    public interface ISelection<T>
    {
        IList<IChromosome<T>> SelectChromosomes(int number, Generation<T> generation);
    }
}
