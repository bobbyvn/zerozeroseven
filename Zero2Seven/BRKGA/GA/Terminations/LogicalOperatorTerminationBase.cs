using System;
using System.Collections.Generic;
using System.Linq;
using BRKGA.Interface;
using HelperSharp;

namespace BRKGA.GA.Terminations
{
    public abstract class LogicalOperatorTerminationBase<T> : ITermination<T>
    {
        protected LogicalOperatorTerminationBase(int minOperands)
        {
            _minOperands = minOperands;
            Terminations = new List<ITermination<T>>();
        }

        protected LogicalOperatorTerminationBase(params ITermination<T>[] terminations)
            : this(2)
        {
            if (terminations != null)
            {
                foreach (var t in terminations)
                {
                    AddTermination(t);
                }
            }
        }

        protected IList<ITermination<T>> Terminations { get; private set; }


        public void AddTermination(ITermination<T> termination)
        {
            ExceptionHelper.ThrowIfNull("termination", termination);

            Terminations.Add(termination);
        }

        public bool HasReached(IGeneticAlgorithm<T> geneticAlgorithm)
        {
            ExceptionHelper.ThrowIfNull("geneticAlgorithm", geneticAlgorithm);

            if (Terminations.Count < _minOperands)
            {
                throw new InvalidOperationException("The {0} needs at least {1} terminations to perform. Please, add the missing terminations.".With(GetType().Name, _minOperands));
            }

            return PerformHasReached(geneticAlgorithm);
        }

        public override string ToString()
        {
            return "{0} ({1})".With(GetType().Name, String.Join(", ", Terminations.Select(t => t.ToString()).ToArray()));
        }

        protected abstract bool PerformHasReached(IGeneticAlgorithm<T> geneticAlgorithm);

        private readonly int _minOperands;
    }
}
