namespace SharedKernel.Domain.Extensions;

/// <summary> List extensions. </summary>
public static class ListExtensions
{
    /// <summary> Get the previus value of a list. </summary>
    public static T Previous<T>(this List<T> list, T current) where T : notnull
    {
        var index = list.IndexOf(current);
        return index == 0 ? default! : list[index - 1];
    }

    /// <summary> Get the next value of the list. </summary>
    public static T Next<T>(this List<T> list, T current) where T : notnull
    {
        var index = list.IndexOf(current);
        return index + 1 >= list.Count ? default! : list[index + 1];
    }
}
