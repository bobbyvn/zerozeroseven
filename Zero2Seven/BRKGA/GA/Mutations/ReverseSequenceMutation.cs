using System.Linq;
using BRKGA.Exception;
using BRKGA.Interface;
using HelperSharp;

namespace BRKGA.GA.Mutations
{
    public class ReverseSequenceMutation<T> : MutationBase<T>
    {
        public ReverseSequenceMutation(IRandomization randomization)
        {
            IsOrdered = true;
            _randomization = randomization;
        }

        protected override void PerformMutate(IChromosome<T> chromosome, float probability)
        {
            if (chromosome.Length < 3)
            {
                throw new MutationException<T>(this, "A chromosome should have, at least, 3 genes. {0} has only {1} gene.".With(chromosome.GetType().Name, chromosome.Length));
            }

            if (_randomization.GetDouble() <= probability)
            {
                var indexes = _randomization.GetUniqueInts(2, 0, chromosome.Length).OrderBy(i => i).ToArray();
                var firstIndex = indexes[0];
                var secondIndex = indexes[1];

                var revertedSequence = chromosome.GetGenes().Skip(firstIndex).Take((secondIndex - firstIndex) + 1).Reverse().ToArray();

                chromosome.ReplaceGenes(firstIndex, revertedSequence);
            }
        }

        private readonly IRandomization _randomization;
    }
}
