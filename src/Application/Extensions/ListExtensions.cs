using System.Collections.Generic;

namespace SharedKernel.Application.Extensions
{
    /// <summary>
    /// List extensions
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Get the previus value of a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public static T Previous<T>(this List<T> list, T current)
        {
            var index = list.IndexOf(current);
            return index == 0 ? default : list[index - 1];
        }

        /// <summary>
        /// Get the next value of the list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public static T Next<T>(this List<T> list, T current)
        {
            var index = list.IndexOf(current);
            return index + 1 >= list.Count ? default : list[index + 1];
        }
    }
}
