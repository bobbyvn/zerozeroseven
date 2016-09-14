using System;
using BRKGA.Interface;
using HelperSharp;

namespace BRKGA.GA
{
    public abstract class ChromosomeBase<T> : IChromosome<T>
    {
        protected ChromosomeBase(int length)
        {
            ValidateLength(length);

            _length = length;
            _genes = new T[length];
        }

        public double? Fitness { get; set; }
        public int Length
        {
            get
            {
                return _length;
            }
        }

        public static bool operator ==(ChromosomeBase<T> first, ChromosomeBase<T> second)
        {
            if (Object.ReferenceEquals(first, second))
            {
                return true;
            }

            if (((object)first == null) || ((object)second == null))
            {
                return false;
            }

            return first.CompareTo(second) == 0;
        }

        public static bool operator !=(ChromosomeBase<T> first, ChromosomeBase<T> second)
        {
            return !(first == second);
        }

        public static bool operator <(ChromosomeBase<T> first, ChromosomeBase<T> second)
        {
            if (Object.ReferenceEquals(first, second))
            {
                return false;
            }
            else if ((object)first == null)
            {
                return true;
            }
            else if ((object)second == null)
            {
                return false;
            }

            return first.CompareTo(second) < 0;
        }

        public static bool operator >(ChromosomeBase<T> first, ChromosomeBase<T> second)
        {
            return !(first == second) && !(first < second);
        }

        public abstract T GenerateGene(int geneIndex);

        public abstract IChromosome<T> CreateNew();

        public virtual IChromosome<T> Clone()
        {
            var clone = CreateNew();
            clone.ReplaceGenes(0, GetGenes());
            clone.Fitness = Fitness;

            return clone;
        }

        public void ReplaceGene(int index, T gene)
        {
            if (index < 0 || index >= _length)
            {
                throw new ArgumentOutOfRangeException("index", "There is no Gene on index {0} to be replaced.".With(index));
            }

            _genes[index] = gene;
            Fitness = null;
        }

        public void ReplaceGenes(int startIndex, T[] genes)
        {
            ExceptionHelper.ThrowIfNull("genes", genes);

            if (genes.Length > 0)
            {
                if (startIndex < 0 || startIndex >= _length)
                {
                    throw new ArgumentOutOfRangeException("index", "There is no Gene on index {0} to be replaced.".With(startIndex));
                }

                var genesToBeReplacedLength = genes.Length;

                var availableSpaceLength = _length - startIndex;

                if (genesToBeReplacedLength > availableSpaceLength)
                {
                    throw new ArgumentException(
                        "genes", "The number of genes to be replaced is greater than available space, there is {0} genes between the index {1} and the end of chromosome, but there is {2} genes to be replaced.".With(availableSpaceLength, startIndex, genesToBeReplacedLength));
                }

                Array.Copy(genes, 0, _genes, startIndex, genes.Length);

                Fitness = null;
            }
        }

        public void Resize(int newLength)
        {
            ValidateLength(newLength);

            Array.Resize(ref _genes, newLength);
            _length = newLength;
        }

        public T GetGene(int index)
        {
            return _genes[index];
        }

        public T[] GetGenes()
        {
            return _genes;
        }

        public int CompareTo(IChromosome<T> other)
        {
            if (other == null)
            {
                return -1;
            }

            var otherFitness = other.Fitness;

            if (Fitness == otherFitness)
            {
                return 0;
            }

            // TODO: chromosomes with same fitnesss are really equals?
            return Fitness > otherFitness ? 1 : -1;
        }

        public override bool Equals(object obj)
        {
            var other = obj as IChromosome<T>;

            if (other == null)
            {
                return false;
            }

            return CompareTo(other) == 0;
        }

        public override int GetHashCode()
        {
            return Fitness.GetHashCode();
        }

        protected virtual void CreateGene(int index)
        {
            ReplaceGene(index, GenerateGene(index));
        }

        protected virtual void CreateGenes()
        {
            for (int i = 0; i < Length; i++)
            {
                ReplaceGene(i, GenerateGene(i));
            }
        }

        private static void ValidateLength(int length)
        {
            if (length < 2)
            {
                throw new ArgumentException("The minimum length for a chromosome is 2 genes.");
            }
        }

        private T[] _genes;
        private int _length;
    }
}
