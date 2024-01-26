namespace SharedKernel.Testing.Architecture;

internal static class CommonExtensionTests
{
    public static TestResult BeSealedAndNotPublicEndingWith(this Types types, Type type, string endsWith)
    {
        return types
            .That()
            .Inherit(type)
            .Should()
            .NotBeAbstract()
            .And()
            .BeSealed()
            .And()
            .NotBePublic()
            .And()
            .HaveNameEndingWith(endsWith)
            .GetResult();
    }
}

