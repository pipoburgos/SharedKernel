using System;

namespace SharedKernel.Application.Utils.Maths
{
    public static class MathUtils
    {
        public static double GetPercentage(int num, int total)
        {
            return Math.Round(total == default ? 0 : (double) num / total * 100, 2);
        }
    }
}