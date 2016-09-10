using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRKGA.Interface
{
    public interface ICrossover<T> : IChromosomeOperator
    {
        int ParentsNumber { get; }
        int ChildrenNumber { get; }
        List<IChromosome<T>> Cross(IList<IChromosome<T>> parents);
    }
}
