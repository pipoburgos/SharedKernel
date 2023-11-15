using SharedKernel.Domain.ValueObjects.PersonalDocumentation;

namespace SharedKernel.Domain.Tests.ValueObjects.PersonalDocumentation;

/// <summary>
/// https://www.generador-de-dni.com/generador-de-dni-avanzado
/// </summary>
public class CifTests
{
    [Theory]
    [InlineData("P5250781A")]
    [InlineData("F15321789")]
    [InlineData("P1428898I")]
    [InlineData("G49196710")]
    [InlineData("G19645266")]
    [InlineData("F40452138")]
    [InlineData("V07097553")]
    [InlineData("A91843326")]
    [InlineData("G14070569")]
    [InlineData("N3975934E")]
    [InlineData("V51395655")]
    [InlineData("V97496863")]
    [InlineData("C21288360")]
    [InlineData("E20699112")]
    [InlineData("V74825936")]
    [InlineData("Q3264582B")]
    [InlineData("R7184403I")]
    [InlineData("J42329169")]
    [InlineData("C00813592")]
    [InlineData("E20181988")]
    [InlineData("F34574830")]
    [InlineData("A98339039")]
    [InlineData("V41828435")]
    [InlineData("V24181018")]
    [InlineData("A34553891")]
    [InlineData("C34503771")]
    [InlineData("G31486392")]
    [InlineData("J44448785")]
    [InlineData("E32316556")]
    [InlineData("W1312065D")]
    [InlineData("N2245376E")]
    [InlineData("C10970879")]
    [InlineData("S4979925G")]
    [InlineData("J22078919")]
    [InlineData("U26730192")]
    [InlineData("R4172470I")]
    [InlineData("H37581535")]
    [InlineData("N1759472B")]
    [InlineData("Q0103691B")]
    [InlineData("P1310109B")]
    [InlineData("G47487152")]
    [InlineData("G18074443")]
    [InlineData("H42497651")]
    [InlineData("F47155213")]
    [InlineData("D56348675")]
    [InlineData("G02776045")]
    [InlineData("A47620166")]
    [InlineData("H42788232")]
    [InlineData("B25363029")]
    [InlineData("S4066483A")]
    public void CifOks(string value)
    {
        Cif.Create(value).IsValid().Should().BeTrue();
    }

    [Theory]
    [InlineData("a")]
    [InlineData("S4066483b")]
    public void CifKos(string value)
    {
        Cif.Create(value).IsValid().Should().BeFalse();
    }
}