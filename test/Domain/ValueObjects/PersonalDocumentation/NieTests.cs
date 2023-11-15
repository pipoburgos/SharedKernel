using SharedKernel.Domain.ValueObjects.PersonalDocumentation;

namespace SharedKernel.Domain.Tests.ValueObjects.PersonalDocumentation;

/// <summary>
/// https://www.generador-de-dni.com/generador-de-dni-avanzado
/// </summary>
public class NieTests
{
    [Theory]
    [InlineData("X1987029J")]
    [InlineData("Y8445021X")]
    [InlineData("X7968558R")]
    [InlineData("X2695558G")]
    [InlineData("Z0184539S")]
    [InlineData("Z2342601X")]
    [InlineData("Z1511570S")]
    [InlineData("Z7982768W")]
    [InlineData("Y6742616L")]
    [InlineData("Z5984632Z")]
    [InlineData("Y0229867L")]
    [InlineData("Z2682413C")]
    [InlineData("X9217074P")]
    [InlineData("Z0897445J")]
    [InlineData("Y7598823M")]
    [InlineData("Z3664631T")]
    [InlineData("Z7653787J")]
    [InlineData("Y5196080A")]
    [InlineData("X9748628D")]
    [InlineData("Z3965223M")]
    [InlineData("Z1326246W")]
    [InlineData("X8741176A")]
    [InlineData("Y8138074K")]
    [InlineData("Y3572543J")]
    [InlineData("Y9538418X")]
    [InlineData("Y0487598N")]
    [InlineData("X5421934Y")]
    [InlineData("X5966285Q")]
    [InlineData("Y9012933Y")]
    [InlineData("X2376147V")]
    [InlineData("X7591133Y")]
    [InlineData("Y4180901K")]
    [InlineData("Z6574376S")]
    [InlineData("X1929051H")]
    [InlineData("Z6705358N")]
    [InlineData("X2310874H")]
    [InlineData("X2406323V")]
    [InlineData("Y1872916V")]
    [InlineData("X2960451Y")]
    [InlineData("X6141419M")]
    [InlineData("Z5880804P")]
    [InlineData("Y1723561R")]
    [InlineData("X7718786D")]
    [InlineData("Z5469917Q")]
    [InlineData("X1103205X")]
    [InlineData("X7388713D")]
    [InlineData("Z3186455V")]
    [InlineData("Y8262547H")]
    [InlineData("X8551926C")]
    [InlineData("Z8167370Y")]
    public void NieOks(string value)
    {
        Nie.Create(value).IsValid().Should().BeTrue();
    }

    [Theory]
    [InlineData("a")]
    [InlineData("S4066483b")]
    public void NieKos(string value)
    {
        Nie.Create(value).IsValid().Should().BeFalse();
    }
}