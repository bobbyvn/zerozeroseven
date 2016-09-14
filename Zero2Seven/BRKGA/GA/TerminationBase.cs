using BRKGA.Interface;
using HelperSharp;

namespace BRKGA.GA
{
    public abstract class TerminationBase<T> : ITermination<T>
    {
        public bool HasReached(IGeneticAlgorithm<T> geneticAlgorithm)
        {
            ExceptionHelper.ThrowIfNull("geneticAlgorithm", geneticAlgorithm);

            _hasReached = PerformHasReached(geneticAlgorithm);

            return _hasReached;
        }

        public override string ToString()
        {
            return "{0} (HasReached: {1})".With(GetType().Name, _hasReached);
        }

        protected abstract bool PerformHasReached(IGeneticAlgorithm<T> geneticAlgorithm);

        private bool _hasReached;
    }
}
