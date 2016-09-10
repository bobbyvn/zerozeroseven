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
    }
}
