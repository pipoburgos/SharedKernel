using NSubstitute;
using SharedKernel.Application.System;
using SharedKernelInfrastructure.iText;

namespace SharedKernel.Integration.Tests.Security;

public class ElectronicSignatureValidatorTests
{
    const string SignedPath = "Security/SignedPdf.pdf";
    const string UnsignedPath = "Security/UnsignedPdf.pdf";
    private readonly IDateTime _dateTime = Substitute.For<IDateTime>();

    public ElectronicSignatureValidatorTests()
    {
        _dateTime.UtcNow.Returns(new DateTime(2022, 5, 25));
    }


    [Fact]
    public async Task ValidateSignature()
    {
        // Arrange
        var validador = new ElectronicSignatureValidator(_dateTime);

        // Act
        var result = await validador
            .Validate(SignedPath, nif: "71282103C", cancellationToken: CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateSignatureNifAndSerialNumber()
    {
        // Arrange
        _dateTime.UtcNow.Returns(new DateTime(2022, 5, 25));
        var validador = new ElectronicSignatureValidator(_dateTime);

        // Act
        var result = await validador
            .Validate(SignedPath, "7583AA0D63911AFB627261B56F9D2F7A", "71282103C", CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateSignatureNotFound()
    {
        // Arrange
        var validador = new ElectronicSignatureValidator(_dateTime);

        // Act
        var result = await validador
            .Validate(SignedPath, nif: "11282103C", cancellationToken: CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateSignatureBytesArray()
    {
        // Arrange
        var contenido = await File.ReadAllBytesAsync(SignedPath);
        var validador = new ElectronicSignatureValidator(_dateTime);

        // Act
        var result = await validador
            .Validate(contenido, nif: "71282103C", cancellationToken: CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateSignatureNotSigned()
    {
        // Arrange
        var contenido = await File.ReadAllBytesAsync(UnsignedPath);
        var validador = new ElectronicSignatureValidator(_dateTime);

        // Act
        var result = await validador
            .Validate(contenido, cancellationToken: CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }
}
