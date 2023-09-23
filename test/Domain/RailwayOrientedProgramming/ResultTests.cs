using SharedKernel.Domain.RailwayOrientedProgramming;
using SharedKernel.Domain.ValueObjects;

namespace SharedKernel.Domain.Tests.RailwayOrientedProgramming;

public class Path : ValueObject<Path>
{
    protected Path() { }

    protected Path(string value) : this()
    {
        Value = value;
    }

    public static Result<Path> CreateResult(string value)
    {
        return Result
            .Create(new Path(value))
            .Ensure(x => !string.IsNullOrWhiteSpace(value) && Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out _),
                Error.Create("Invalid Path", nameof(Path)));
    }

    public string Value { get; private set; } = null!;
}

public class ResultTests
{
    [Fact]
    public void NullableResultIsSuccess()
    {
        string text = null!;
        var path = string.IsNullOrWhiteSpace(text) ? Result.Empty<Path>()! : Path.CreateResult(string.Empty);

        path.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void NullableResultIsSuccess2()
    {
        CreateNull().IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void DefaultResultIsSuccess()
    {
        CreateDefault().IsSuccess.Should().BeTrue();
    }

    private Result<Path?> CreateNull() => null;

    private Result<Path?> CreateDefault() => null;
}
