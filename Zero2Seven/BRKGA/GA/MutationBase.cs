using BRKGA.Interface;
using HelperSharp;

namespace BRKGA.GA
{
    public abstract class MutationBase<T> : IMutation<T>
    {
        public bool IsOrdered { get; protected set; }
        public void Mutate(IChromosome<T> chromosome, float probability)
        {
            ExceptionHelper.ThrowIfNull("chromosome", chromosome);

            PerformMutate(chromosome, probability);
        }

        protected abstract void PerformMutate(IChromosome<T> chromosome, float probability);
    }
}
