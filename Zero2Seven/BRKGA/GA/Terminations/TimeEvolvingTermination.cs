using System;
using BRKGA.Interface;

namespace BRKGA.GA.Terminations
{
    public class TimeEvolvingTermination<T> : TerminationBase<T>
    {
        public TimeEvolvingTermination() : this(TimeSpan.FromMinutes(1))
        {
        }

        public TimeEvolvingTermination(TimeSpan maxTime)
        {
            MaxTime = maxTime;
        }

        public TimeSpan MaxTime { get; set; }

        protected override bool PerformHasReached(IGeneticAlgorithm<T> geneticAlgorithm)
        {
            return geneticAlgorithm.TimeEvolving >= MaxTime;
        }
    }
}
