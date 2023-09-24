using SharedKernel.Domain.ValueObjects.PersonalDocumentation;

namespace SharedKernel.Domain.Tests.ValueObjects.PersonalDocumentation
{
    /// <summary>
    /// https://www.generador-de-dni.com/generador-de-dni-avanzado
    /// </summary>
    public class NifTests
    {
        [Theory]
        [InlineData("15261150Y")]
        [InlineData("51146009N")]
        [InlineData("78358593P")]
        [InlineData("68829913K")]
        [InlineData("34577172F")]
        [InlineData("22561148B")]
        [InlineData("70973426A")]
        [InlineData("51243229B")]
        [InlineData("54585541D")]
        [InlineData("41540962H")]
        [InlineData("25658798K")]
        [InlineData("06046977R")]
        [InlineData("95961805D")]
        [InlineData("15177187Q")]
        [InlineData("80068965P")]
        [InlineData("84683283C")]
        [InlineData("91855156X")]
        [InlineData("73752923L")]
        [InlineData("29020328D")]
        [InlineData("19530979T")]
        [InlineData("50403256K")]
        [InlineData("39489232A")]
        [InlineData("66386906M")]
        [InlineData("52419211A")]
        [InlineData("43068786K")]
        [InlineData("81830323A")]
        [InlineData("81381825M")]
        [InlineData("82091789M")]
        [InlineData("88365938F")]
        [InlineData("61586851G")]
        [InlineData("36648685W")]
        [InlineData("82923111S")]
        [InlineData("90727630J")]
        [InlineData("68848005N")]
        [InlineData("84747431K")]
        [InlineData("55415875C")]
        [InlineData("61346523A")]
        [InlineData("44357029B")]
        [InlineData("20611230X")]
        [InlineData("28235999A")]
        [InlineData("04686598A")]
        [InlineData("63028785E")]
        [InlineData("36588303H")]
        [InlineData("21146911K")]
        [InlineData("75774382P")]
        [InlineData("90755360M")]
        [InlineData("27341164Y")]
        [InlineData("58397915H")]
        [InlineData("53769413J")]
        [InlineData("31377694J")]
        public void NifOks(string value)
        {
            Nif.Create(value).IsValid().Should().BeTrue();
        }

        [Theory]
        [InlineData("a")]
        [InlineData("S4066483b")]
        public void NieKos(string value)
        {
            Nif.Create(value).IsValid().Should().BeFalse();
        }
    }
}
