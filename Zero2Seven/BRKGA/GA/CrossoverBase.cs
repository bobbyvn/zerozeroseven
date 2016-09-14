using System;
using System.Collections.Generic;
using BRKGA.Exception;
using BRKGA.Interface;
using HelperSharp;

namespace BRKGA.GA
{
    public  abstract class CrossoverBase<T> : ICrossover<T>
    {
        public bool IsOrdered { get; protected set; }
        public int ParentsNumber { get; private set; }
        public int ChildrenNumber { get; private set; }
        public int MinChromosomeLength { get; protected set; }

        public IList<IChromosome<T>> Cross(IList<IChromosome<T>> parents)
        {
            ExceptionHelper.ThrowIfNull("parents", parents);
            if (parents.Count != ParentsNumber)
            {
                throw new ArgumentOutOfRangeException("parents", "The number of parents should be the same of ParentsNumber.");
            }

            var firstParent = parents[0];

            if (firstParent.Length < MinChromosomeLength)
            {
                throw new CrossoverException<T>(
                    this, "A chromosome should have, at least, {0} genes. {1} has only {2} gene.".With(MinChromosomeLength, firstParent.GetType().Name, firstParent.Length));
            }

            return PerformCross(parents);
        }

        protected CrossoverBase(int parentsNumber, int childrenNumber) : this(parentsNumber, childrenNumber, 2)
        {
            ParentsNumber = parentsNumber;
            ChildrenNumber = childrenNumber;
        }

        protected CrossoverBase(int parentsNumber, int childrenNumber, int minChromosomeLength)
        {
            ParentsNumber = parentsNumber;
            ChildrenNumber = childrenNumber;
            MinChromosomeLength = minChromosomeLength;
        }

        protected abstract IList<IChromosome<T>> PerformCross(IList<IChromosome<T>> parents);
    }
}
