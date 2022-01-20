using FluentAssertions;
using SharedKernel.Domain.ValueObjects.PersonalDocumentation;
using Xunit;

namespace SharedKernel.Domain.Tests.ValueObjects.PersonalDocumentation
{
    /// <summary>
    /// https://www.generador-de-dni.com/generador-de-dni-avanzado
    /// </summary>
    public class DniTests
    {
        [Theory]
        [InlineData("11982607K")]
        [InlineData("65480128W")]
        [InlineData("96422683J")]
        [InlineData("19231998L")]
        [InlineData("56213115B")]
        [InlineData("04637533C")]
        [InlineData("13912192K")]
        [InlineData("39121452Q")]
        [InlineData("61226280G")]
        [InlineData("11235905Z")]
        [InlineData("21150365W")]
        [InlineData("32661571F")]
        [InlineData("63220292P")]
        [InlineData("76117598H")]
        [InlineData("48382007N")]
        [InlineData("63643566J")]
        [InlineData("71646587T")]
        [InlineData("26511991Y")]
        [InlineData("19547288W")]
        [InlineData("22721280V")]
        [InlineData("87006877Q")]
        [InlineData("45824539Y")]
        [InlineData("91594967C")]
        [InlineData("46307056Y")]
        [InlineData("91839059J")]
        [InlineData("36284922F")]
        [InlineData("90839411Z")]
        [InlineData("56297327C")]
        [InlineData("28940938S")]
        [InlineData("59024267B")]
        [InlineData("57124274W")]
        [InlineData("28029859N")]
        [InlineData("79497490Z")]
        [InlineData("93055753F")]
        [InlineData("49694339X")]
        [InlineData("37267849Y")]
        [InlineData("49199952P")]
        [InlineData("11831552F")]
        [InlineData("62846494Y")]
        [InlineData("88782908X")]
        [InlineData("36860824N")]
        [InlineData("70697774Y")]
        [InlineData("11399728P")]
        [InlineData("68177585L")]
        [InlineData("98532072A")]
        [InlineData("85637469M")]
        [InlineData("14301388B")]
        [InlineData("60013365V")]
        [InlineData("80838422T")]
        [InlineData("23241986A")]
        public void DniOks(string value)
        {
            Dni.Create(value).IsValid().Should().BeTrue();
        }

        [Theory]
        [InlineData("a")]
        [InlineData("S4066483b")]
        public void DniKos(string value)
        {
            Dni.Create(value).IsValid().Should().BeFalse();
        }
    }
}
