using System.Linq;
using BRKGA.Exception;
using BRKGA.Interface;
using HelperSharp;

namespace BRKGA.GA.Mutations
{
    public class UniformMutation<T> : MutationBase<T>
    {
        public UniformMutation(IRandomization randomization, params int[] mutableGenesIndexes)
        {
            _mutableGenesIndexes = mutableGenesIndexes;
            _randomization = randomization;
        }

        public UniformMutation(bool allGenesMutable)
        {
            _allGenesMutable = allGenesMutable;
        }

        public UniformMutation() : this(false)
        {
        }

        protected override void PerformMutate(IChromosome<T> chromosome, float probability)
        {
            ExceptionHelper.ThrowIfNull("chromosome", chromosome);

            var genesLength = chromosome.Length;

            if (_mutableGenesIndexes == null || _mutableGenesIndexes.Length == 0)
            {
                if (_allGenesMutable)
                {
                    _mutableGenesIndexes = Enumerable.Range(0, genesLength).ToArray();
                }
                else
                {
                    _mutableGenesIndexes = _randomization.GetInts(1, 0, genesLength);
                }
            }

            foreach (var i in _mutableGenesIndexes)
            {
                if (i >= genesLength)
                {
                    throw new MutationException<T>(this, "The chromosome has no gene on index {0}. The chromosome genes length is {1}.".With(i, genesLength));
                }

                if (_randomization.GetDouble() <= probability)
                {
                    chromosome.ReplaceGene(i, chromosome.GenerateGene(i));
                }
            }
        }

        private readonly bool _allGenesMutable;
        private int[] _mutableGenesIndexes;
        private readonly IRandomization _randomization;
    }
}
