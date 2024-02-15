namespace SharedKernel.Testing.Architecture;

public static class CommonExtensionTests
{
    public static TestResult ClassBeSealedAndNotPublicEndingWith(this Types types, Type type, string endsWith)
    {
        return types
            .That()
            .Inherit(type)
            .And()
            .AreNotAbstract()
            .Should()
            .BeSealed()
            .And()
            .NotBePublic()
            .And()
            .HaveNameEndingWith(endsWith)
            .GetResult();
    }

    public static TestResult InterfaceBeSealedAndNotPublicEndingWith(this Types types, Type type, string endsWith)
    {
        return types
            .That()
            .ImplementInterface(type)
            .And()
            .AreNotAbstract()
            .Should()
            .BeSealed()
            .And()
            .NotBePublic()
            .And()
            .HaveNameEndingWith(endsWith)
            .GetResult();
    }

    public static TestResult InterfaceBeSealed(this Types types, Type type)
    {
        return types
            .That()
            .ImplementInterface(type)
            .And()
            .AreNotAbstract()
            .Should()
            .BeSealed()
            .GetResult();
    }
}

