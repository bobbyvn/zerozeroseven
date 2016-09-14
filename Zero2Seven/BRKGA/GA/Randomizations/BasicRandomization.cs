using System;
using System.Threading;

namespace BRKGA.GA.Randomizations
{
    public class BasicRandomization : RandomizationBase
    {
        public override int GetInt(int min, int max)
        {
            return Random.Next(min, max);
        }

        public override float GetFloat()
        {
            return (float)Random.NextDouble();
        }

        public override double GetDouble()
        {
            return Random.NextDouble();
        }

        private static Random Random
        {
            get
            {
                if (_random == null)
                {
                    _random = new Random(Interlocked.Increment(ref _seed));
                }

                return _random;
            }
        }

        [ThreadStatic]
        private static Random _random;
        private static int _seed = Environment.TickCount;
    }
}
