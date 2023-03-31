using BankAccounts.Domain.BankAccounts.Specifications;
using BankAccounts.Domain.Tests.Data;
using FluentAssertions;
using Xunit;

namespace BankAccounts.Domain.Tests.Specifications
{
    public class IsASpanishBankAccountSpecTests
    {
        [Fact]
        public void IsSpanishAccount()
        {
            // Arrange
            var bankAccount =
                BankAccountTestFactory.Create(iban: InternationalBankAccountNumberTestFactory.Create("ES14"));

            // Act
            var isSpanish = new IsASpanishBankAccountSpec().SatisfiedBy().Compile()(bankAccount);

            // Assert
            isSpanish.Should().BeTrue();
        }

        [Fact]
        public void IsNotSpanishAccount()
        {
            // Arrange
            var bankAccount =
                BankAccountTestFactory.Create(iban: InternationalBankAccountNumberTestFactory.Create("DE14"));

            // Act
            var isSpanish = new IsASpanishBankAccountSpec().SatisfiedBy().Compile()(bankAccount);

            // Assert
            isSpanish.Should().BeFalse();
        }
    }
}
