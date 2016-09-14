using System;
using BRKGA.Helper;

namespace BRKGA.GA.Randomizations
{
    public class FastRandomRandomization : RandomizationBase
    {
        public override int GetInt(int min, int max)
        {
            return _random.Next(min, max);
        }

        public override float GetFloat()
        {
            return (float)_random.NextDouble();
        }

        public override double GetDouble()
        {
            return _random.NextDouble();
        }

        private readonly FastRandomHelper _random = new FastRandomHelper(DateTime.Now.Millisecond);
    }
}
