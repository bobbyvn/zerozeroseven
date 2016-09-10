using System;
using System.Collections.Generic;
using System.Linq;
using BRKGA.Model;

namespace BRKGA.Helper
{
    public class EmptySpaceHelper
    {
        public static List<EmptySpace> GetEmptySpaces(EmptySpace source, EmptySpace target)
        {
            return new List<EmptySpace>
            {
                new EmptySpace(source.StartX, source.StartY, source.StartZ, target.StartX, source.EndY, source.EndZ),
                new EmptySpace(source.StartX, source.StartY, source.StartZ, source.EndX, target.StartY, source.EndZ),
                new EmptySpace(source.StartX, source.StartY, source.StartZ, source.EndX, target.EndY, target.StartX),
                new EmptySpace(source.StartX, source.StartY, target.EndZ, source.EndX, source.EndY, source.EndZ),
                new EmptySpace(source.StartX, target.EndY, source.StartZ, source.EndX, source.EndY, source.EndZ),
                new EmptySpace(target.EndX, source.StartY, source.StartZ, source.EndX, source.EndY, source.EndZ)
            };
        }

        public static List<EmptySpace> UpdateEmptySpaces(List<EmptySpace> spaces, Box box)
        {
            var tS = new List<EmptySpace>();
            var nS = new List<EmptySpace>();
            var boxSpace = box.GetSpace();

            foreach (var space in spaces)
            {
                if (boxSpace.StartX >= space.EndX || boxSpace.EndX <= space.StartX || boxSpace.StartY > space.StartY || boxSpace.EndY <= space.StartY || boxSpace.StartZ >= space.EndZ || boxSpace.EndZ <= space.StartZ)
                {
                    tS.Add(space);
                }
                else
                {
                    nS = GetEmptySpaces(space, boxSpace);
                }
            }

            nS = EliminateEmptySpaces(nS);
            tS = EliminateEmptySpaces(tS);
            nS.AddRange(tS);

            return nS;
        }

        private static List<EmptySpace> EliminateEmptySpaces(List<EmptySpace> spaces)
        {
            spaces.RemoveAll(p => p.StartX - p.EndX <= Double.Epsilon || p.StartY - p.EndY <= Double.Epsilon || p.StartZ - p.EndZ <= Double.Epsilon);
            return CrossChecking(spaces);
        }

        private static List<EmptySpace> CrossChecking(List<EmptySpace> spaces)
        {
            int index = 1;
            while (index < spaces.Count)
            {
                if (spaces.Skip(index).Any(p => IsCrossing(p, spaces[index -1])))
                {
                    spaces = spaces.Skip(1).ToList();
                }
                else
                {
                    index ++;
                }
            }
            
            return spaces;
        }

        private static bool IsCrossing(EmptySpace s1, EmptySpace s2)
        {
            return s1.StartX <= s2.StartX && s1.StartY <= s2.StartY && s1.StartZ <= s2.StartX && s1.EndX >= s2.EndX &&
                   s1.EndY >= s2.EndY && s1.EndZ >= s2.EndZ;
        }
    }
}
