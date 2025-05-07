using SharedKernel.Domain.Extensions;

namespace SharedKernel.Domain.Tests.Extensions;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("helloWorld", "helloWorld")]
    [InlineData("hello world", "helloWorld")]
    [InlineData("Hello World", "helloWorld")]
    [InlineData("hello-world", "helloWorld")]
    [InlineData("hello_world", "helloWorld")]
    [InlineData("Hello-World", "helloWorld")]
    [InlineData("hello", "hello")]
    [InlineData("", "")]
    [InlineData("   ", "")]
    public void ToCamelCaseTests(string input, string expected)
    {
        input.ToCamelCase().Should().Be(expected);
    }

    [Theory]
    [InlineData("HelloWorld", "HelloWorld")]
    [InlineData("hello world", "HelloWorld")]
    [InlineData("Hello World", "HelloWorld")]
    [InlineData("hello-world", "HelloWorld")]
    [InlineData("hello_world", "HelloWorld")]
    [InlineData("Hello-World", "HelloWorld")]
    [InlineData("hello", "Hello")]
    [InlineData("", "")]
    [InlineData("   ", "")]
    public void ToPascalCaseTests(string input, string expected)
    {
        input.ToPascalCase().Should().Be(expected);
    }

    [Theory]
    [InlineData("hello-world", "hello-world")]
    [InlineData("hello world", "hello-world")]
    [InlineData("HelloWorld", "hello-world")]
    [InlineData("helloWorld", "hello-world")]
    [InlineData("hello_world", "hello-world")]
    [InlineData("Hello-World", "hello-world")]
    [InlineData("hello", "hello")]
    [InlineData("", "")]
    [InlineData("   ", "")]
    public void ToKebabCaseTests(string input, string expected)
    {
        input.ToKebabCase().Should().Be(expected);
    }

    [Theory]
    [InlineData("hello_world", "hello_world")]
    [InlineData("hello world", "hello_world")]
    [InlineData("HelloWorld", "hello_world")]
    [InlineData("helloWorld", "hello_world")]
    [InlineData("hello-world", "hello_world")]
    [InlineData("Hello-World", "hello_world")]
    [InlineData("hello", "hello")]
    [InlineData("", "")]
    [InlineData("   ", "")]
    public void ToSnakeCaseTests(string input, string expected)
    {
        input.ToSnakeCase().Should().Be(expected);
    }

    [Theory]
    [InlineData("hello-World", "Hello-World")]
    [InlineData("hello world", "Hello-World")]
    [InlineData("HelloWorld", "Hello-World")]
    [InlineData("helloWorld", "Hello-World")]
    [InlineData("hello-world", "Hello-World")]
    [InlineData("hello_world", "Hello-World")]
    [InlineData("Hello-World", "Hello-World")]
    [InlineData("hello", "Hello")]
    [InlineData("", "")]
    [InlineData("   ", "")]
    public void ToTrainCaseTests(string input, string expected)
    {
        input.ToTrainCase().Should().Be(expected);
    }
}