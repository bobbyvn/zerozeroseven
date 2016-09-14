using System.Collections.Generic;
using System.Linq;
using BRKGA.Interface;

namespace BRKGA.GA.Crossovers
{
    public class CutAndSpliceCrossover<T> : CrossoverBase<T>
    {
        public CutAndSpliceCrossover(IRandomization randomization)
            : base(2, 2)
        {
            IsOrdered = false;
            _randomization = randomization;
        }

        protected override IList<IChromosome<T>> PerformCross(IList<IChromosome<T>> parents)
        {
            var parent1 = parents[0];
            var parent2 = parents[1];

            // The minium swap point is 1 to safe generate a gene with at least two genes.
            var parent1Point = _randomization.GetInt(1, parent1.Length) + 1;
            var parent2Point = _randomization.GetInt(1, parent2.Length) + 1;

            var offspring1 = CreateOffspring(parent1, parent2, parent1Point, parent2Point);
            var offspring2 = CreateOffspring(parent2, parent1, parent2Point, parent1Point);

            return new List<IChromosome<T>>() { offspring1, offspring2 };
        }

        private static IChromosome<T> CreateOffspring(IChromosome<T> leftParent, IChromosome<T> rightParent, int leftParentPoint, int rightParentPoint)
        {
            var offspring = leftParent.CreateNew();

            offspring.Resize(leftParentPoint + (rightParent.Length - rightParentPoint));
            offspring.ReplaceGenes(0, leftParent.GetGenes().Take(leftParentPoint).ToArray());
            offspring.ReplaceGenes(leftParentPoint, rightParent.GetGenes().Skip(rightParentPoint).ToArray());

            return offspring;
        }

        private readonly IRandomization _randomization;
    }
}
