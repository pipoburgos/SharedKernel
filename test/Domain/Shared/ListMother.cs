namespace SharedKernel.Domain.Tests.Shared;

public static class ListMother
{
    public static List<T> Create<T>(int size, Func<T> creator)
    {
        var list = new List<T>();

        for (var i = 0; i < size; i++)
        {
            if (creator != null) list.Add(creator());
        }

        return list;
    }

    public static List<T> Random<T>(Func<T> creator)
    {
        return Create(IntegerMother.Between(1, 10), creator);
    }

    public static List<T> One<T>(T element)
    {
        return [element];
    }
}