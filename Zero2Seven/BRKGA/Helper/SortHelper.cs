using System;
using System.Collections.Generic;
using BRKGA.Model;

namespace BRKGA.Helper
{
    public class SortHelper
    {
        public static void SortByDeepestBottomLeft(List<EmptySpace> spaces)
        {
            bool hasSwap = false;
            int count = spaces.Count;
            for (int i = 0; i < count -1; i++)
            {
                for (int j = count - 1; j > i; j--)
                {
                    if (spaces[j] < spaces[j - 1])
                    {
                        var tmp = spaces[j];
                        spaces[j] = spaces[j - 1];
                        spaces[j - 1] = tmp;
                        hasSwap = true;
                    }
                }

                if (!hasSwap)
                {
                    return;
                }
            }
        }
    }
}
