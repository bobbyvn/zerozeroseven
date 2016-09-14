using BRKGA.Interface;

namespace BRKGA.GA.Mutations
{
    public class TworsMutation<T> : MutationBase<T>
    {
        public TworsMutation(IRandomization randomization)
        {
            IsOrdered = true;
            _randomization = randomization;
        }

        protected override void PerformMutate(IChromosome<T> chromosome, float probability)
        {
            if (_randomization.GetDouble() <= probability)
            {
                var indexes = _randomization.GetUniqueInts(2, 0, chromosome.Length);
                var firstIndex = indexes[0];
                var secondIndex = indexes[1];
                var firstGene = chromosome.GetGene(firstIndex);
                var secondGene = chromosome.GetGene(secondIndex);

                chromosome.ReplaceGene(firstIndex, secondGene);
                chromosome.ReplaceGene(secondIndex, firstGene);
            }
        }

        private readonly IRandomization _randomization;
    }
}
