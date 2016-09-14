using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BRKGA.Interface;
using HelperSharp;

namespace BRKGA.GA
{
    public abstract class RandomizationBase : IRandomization
    {
        public abstract int GetInt(int min, int max);
        public abstract float GetFloat();
        public abstract double GetDouble();
        public virtual int[] GetInts(int length, int min, int max)
        {
            var ints = new int[length];

            for (int i = 0; i < length; i++)
            {
                ints[i] = GetInt(min, max);
            }

            return ints;
        }
        public virtual int[] GetUniqueInts(int length, int min, int max)
        {
            var diff = max - min;

            if (diff < length)
            {
                throw new ArgumentOutOfRangeException(
                    "length",
                    "The length is {0}, but the possible unique values between {1} (inclusive) and {2} (exclusive) are {3}.".With(length, min, max, diff));
            }

            var orderedValues = Enumerable.Range(min, diff).ToList();
            var ints = new int[length];

            for (int i = 0; i < length; i++)
            {
                var removeIndex = GetInt(0, orderedValues.Count);
                ints[i] = orderedValues[removeIndex];
                orderedValues.RemoveAt(removeIndex);
            }

            return ints;
        }

        public float GetFloat(float min, float max)
        {
            return min + ((max - min) * GetFloat());
        }

        public virtual double GetDouble(double min, double max)
        {
            return min + ((max - min) * GetDouble());
        }
    }
}
