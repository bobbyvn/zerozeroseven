using System.Linq;
using BRKGA.Interface;

namespace BRKGA.GA.Terminations
{
    public class OrTermination<T> : LogicalOperatorTerminationBase<T>
    {
        public OrTermination(params ITermination<T>[] terminations)
            : base(terminations)
        {
        }

        protected override bool PerformHasReached(IGeneticAlgorithm<T> geneticAlgorithm)
        {
            return Terminations.Any(t => t.HasReached(geneticAlgorithm));
        }
    }
}
