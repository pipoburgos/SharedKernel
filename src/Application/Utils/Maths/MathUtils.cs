using System;

namespace SharedKernel.Application.Utils.Maths
{
    /// <summary>
    /// 
    /// </summary>
    public static class MathUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="num"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static double GetPercentage(int num, int total)
        {
            return Math.Round(total == default ? 0 : (double) num / total * 100, 2);
        }
    }
}