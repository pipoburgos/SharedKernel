namespace SharedKernel.Domain.Tests.Shared;

public static class RandomElementPicker
{
    public static string From(params string[] values)
    {
        return values[IntegerMother.Between(0, values.Length - 1)];
    }
}