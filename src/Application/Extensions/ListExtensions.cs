using System.Collections.Generic;

namespace SharedKernel.Application.Extensions
{
    public static class ListExtensions
    {
        public static T Previous<T>(this List<T> list, T current)
        {
            var index = list.IndexOf(current);
            return index == 0 ? default : list[index - 1];
        }

        public static T Next<T>(this List<T> list, T current)
        {
            var index = list.IndexOf(current);
            return index + 1 >= list.Count ? default : list[index + 1];
        }
    }
}
