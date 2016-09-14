using System.Collections.Generic;
using BRKGA.Exception;
using BRKGA.Interface;
using HelperSharp;

namespace BRKGA.GA
{
    public abstract class ReinsertionBase<T> : IReinsertion<T>
    {
        protected ReinsertionBase(bool canCollapse, bool canExpand)
        {
            CanCollapse = canCollapse;
            CanExpand = canExpand;
        }

        public bool CanCollapse { get; private set; }

        public bool CanExpand { get; private set; }

        public IList<IChromosome<T>> SelectChromosomes(IPopulation<T> population, IList<IChromosome<T>> offspring, IList<IChromosome<T>> parents)
        {
            ExceptionHelper.ThrowIfNull("population", population);
            ExceptionHelper.ThrowIfNull("offspring", offspring);
            ExceptionHelper.ThrowIfNull("parents", parents);

            if (!CanExpand && offspring.Count < population.MinSize)
            {
                throw new ReinsertionException<T>(this, "Cannot expand the number of chromosome in population. Try another reinsertion!");
            }

            if (!CanCollapse && offspring.Count > population.MaxSize)
            {
                throw new ReinsertionException<T>(this, "Cannot collapse the number of chromosome in population. Try another reinsertion!");
            }

            return PerformSelectChromosomes(population, offspring, parents);
        }

        protected abstract IList<IChromosome<T>> PerformSelectChromosomes(IPopulation<T> population, IList<IChromosome<T>> offspring, IList<IChromosome<T>> parents);
    }
}
