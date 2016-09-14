using System;
using System.Collections.Generic;
using BRKGA.Interface;
using BRKGA.Model;
using HelperSharp;

namespace BRKGA.GA
{
    public abstract class SelectionBase<T> : ISelection<T>
    {
        protected SelectionBase(int minNumberChromosomes)
        {
            _minNumberChromosomes = minNumberChromosomes;
        }

        public IList<IChromosome<T>> SelectChromosomes(int number, Generation<T> generation)
        {
            if (number < _minNumberChromosomes)
            {
                throw new ArgumentOutOfRangeException("number", "The number of selected chromosomes should be at least {0}.".With(_minNumberChromosomes));
            }

            ExceptionHelper.ThrowIfNull("generation", generation);

            return PerformSelectChromosomes(number, generation);
        }

        protected abstract IList<IChromosome<T>> PerformSelectChromosomes(int number, Generation<T> generation);

        private readonly int _minNumberChromosomes;
    }
}
