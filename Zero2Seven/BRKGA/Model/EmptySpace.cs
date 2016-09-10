using System;

namespace BRKGA.Model
{
    public class EmptySpace
    {
        public EmptySpace(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            StartX = x1;
            StartY = y1;
            StartZ = z1;
            EndX = x2;
            EndY = y2;
            EndZ = z2;
        }

        public double StartX { get; private set; }
        public double StartY { get; private set; }
        public double StartZ { get; private set; }
        public double EndX { get; private set; }
        public double EndY { get; private set; }
        public double EndZ { get; private set; }

        public static bool operator <(EmptySpace sp1, EmptySpace sp2)
        {
            return sp1.EndY < sp2.EndY || (sp1.EndY - sp2.EndY <= Double.Epsilon && sp1.EndZ < sp2.EndZ) ||(sp1.EndY - sp2.EndY <= Double.Epsilon && sp1.EndZ - sp2.EndZ <= Double.Epsilon && sp1.EndX < sp2.EndX);
        }

        public static bool operator >(EmptySpace sp1, EmptySpace sp2)
        {
            return sp1.EndY > sp2.EndY || (sp1.EndY - sp2.EndY <= Double.Epsilon && sp1.EndZ > sp2.EndZ) || (sp1.EndY - sp2.EndY <= Double.Epsilon && sp1.EndZ - sp2.EndZ <= Double.Epsilon && sp1.EndX > sp2.EndX);
        }
    }
}
