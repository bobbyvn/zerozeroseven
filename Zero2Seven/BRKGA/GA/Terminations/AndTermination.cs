using System.Linq;
using BRKGA.Interface;

namespace BRKGA.GA.Terminations
{
    public class AndTermination<T> : LogicalOperatorTerminationBase<T>
    {
        public AndTermination(params ITermination<T>[] terminations) : base(terminations)
        {
        }

        protected override bool PerformHasReached(IGeneticAlgorithm<T> geneticAlgorithm)
        {
            return Terminations.All(t => t.HasReached(geneticAlgorithm));
        }
    }
}
