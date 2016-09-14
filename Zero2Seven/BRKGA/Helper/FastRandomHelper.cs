using System;

namespace BRKGA.Helper
{
    public class FastRandomHelper
    {
        public FastRandomHelper(int seed)
        {
            Reinitialize(seed);
        }

        public void Reinitialize(int seed)
        {
            // The only stipulation stated for the xorshift RNG is that at least one of
            // the seeds x,y,z,w is non-zero. We fulfill that requirement by only allowing
            // resetting of the x seed
            _x = (uint)seed;
            _y = Y;
            _z = Z;
            _w = W;
        }

        public int Next(int lowerBound, int upperBound)
        {
            if (lowerBound > upperBound)
                throw new ArgumentOutOfRangeException("upperBound", upperBound, "upperBound must be >=lowerBound");

            uint t = (_x ^ (_x << 11));
            _x = _y; _y = _z; _z = _w;

            // The explicit int cast before the first multiplication gives better performance.
            // See comments in NextDouble.
            int range = upperBound - lowerBound;
            if (range < 0)
            {   // If range is <0 then an overflow has occured and must resort to using long integer arithmetic instead (slower).
                // We also must use all 32 bits of precision, instead of the normal 31, which again is slower.	
                return lowerBound + (int)((RealUnitUint * (double)(_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8)))) * (double)((long)upperBound - (long)lowerBound));
            }

            // 31 bits of precision will suffice if range<=int.MaxValue. This allows us to cast to an int and gain
            // a little more performance.
            return lowerBound + (int)((RealUnitInt * (double)(int)(0x7FFFFFFF & (_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8))))) * (double)range);
        }

        public double NextDouble()
        {
            uint t = (_x ^ (_x << 11));
            _x = _y; _y = _z; _z = _w;

            // Here we can gain a 2x speed improvement by generating a value that can be cast to 
            // an int instead of the more easily available uint. If we then explicitly cast to an 
            // int the compiler will then cast the int to a double to perform the multiplication, 
            // this final cast is a lot faster than casting from a uint to a double. The extra cast
            // to an int is very fast (the allocated bits remain the same) and so the overall effect 
            // of the extra cast is a significant performance improvement.
            //
            // Also note that the loss of one bit of precision is equivalent to what occurs within 
            // System.Random.
            return (RealUnitInt * (int)(0x7FFFFFFF & (_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8)))));
        }

        private const double RealUnitInt = 1.0 / ((double)int.MaxValue + 1.0);
        private const double RealUnitUint = 1.0 / ((double)uint.MaxValue + 1.0);
        private const uint Y = 842502087, Z = 3579807591, W = 273326509;
        private uint _x, _y, _z, _w;
    }
}
