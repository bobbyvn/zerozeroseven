using System.Collections.Generic;
using System.Linq;
using BRKGA.Exception;
using BRKGA.Interface;
using BRKGA.Model;
using HelperSharp;

namespace BRKGA.GA.Selections
{
    public class TournamentSelection<T> : SelectionBase<T>
    {
        public TournamentSelection(IRandomization randomization) : this(randomization, 2)
        {
        }

        public TournamentSelection(IRandomization randomization, int size) : this(randomization, size, true)
        {
        }

        public TournamentSelection(IRandomization randomization, int size, bool allowWinnerCompeteNextTournament) : base(2)
        {
            Size = size;
            AllowWinnerCompeteNextTournament = allowWinnerCompeteNextTournament;
            _randomization = randomization;
        }

        public int Size { get; set; }

        public bool AllowWinnerCompeteNextTournament { get; set; }

        protected override IList<IChromosome<T>> PerformSelectChromosomes(int number, Generation<T> generation)
        {
            if (Size > generation.Chromosomes.Count)
            {
                throw new SelectionException<T>(
                    this,
                    "The tournament size is greater than available chromosomes. Tournament size is {0} and generation {1} available chromosomes are {2}.".With(Size, generation.Number, generation.Chromosomes.Count));
            }

            var candidates = generation.Chromosomes.ToList();
            var selected = new List<IChromosome<T>>();

            while (selected.Count < number)
            {
                var randomIndexes = _randomization.GetUniqueInts(Size, 0, candidates.Count);
                var tournamentWinner = candidates.Where((c, i) => randomIndexes.Contains(i)).OrderByDescending(c => c.Fitness).First();

                selected.Add(tournamentWinner);

                if (!AllowWinnerCompeteNextTournament)
                {
                    candidates.Remove(tournamentWinner);
                }
            }

            return selected;
        }

        protected readonly IRandomization _randomization;
    }
}
