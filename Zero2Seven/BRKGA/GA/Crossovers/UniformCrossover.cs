using System.Collections.Generic;
using BRKGA.Interface;

namespace BRKGA.GA.Crossovers
{
    public class UniformCrossover<T> : CrossoverBase<T>
    {
        public UniformCrossover(IRandomization randomization, float mixProbability)
            : base(2, 2)
        {
            MixProbability = mixProbability;
            _randomization = randomization;
        }

        public UniformCrossover(IRandomization randomization) : this(randomization, 0.5f)
        {
        }

        public float MixProbability { get; set; }

        protected override IList<IChromosome<T>> PerformCross(IList<IChromosome<T>> parents)
        {
            var firstParent = parents[0];
            var secondParent = parents[1];
            var firstChild = firstParent.CreateNew();
            var secondChild = secondParent.CreateNew();

            for (int i = 0; i < firstParent.Length; i++)
            {
                if (_randomization.GetDouble() < MixProbability)
                {
                    firstChild.ReplaceGene(i, firstParent.GetGene(i));
                    secondChild.ReplaceGene(i, secondParent.GetGene(i));
                }
                else
                {
                    firstChild.ReplaceGene(i, secondParent.GetGene(i));
                    secondChild.ReplaceGene(i, firstParent.GetGene(i));
                }
            }

            return new List<IChromosome<T>> { firstChild, secondChild };
        }

        private readonly IRandomization _randomization;
    }
}
