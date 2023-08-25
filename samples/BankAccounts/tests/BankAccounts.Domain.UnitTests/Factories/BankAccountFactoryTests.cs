using BankAccounts.Domain.BankAccounts;
using BankAccounts.Domain.Tests.Data;

// ReSharper disable ExpressionIsAlwaysNull

namespace BankAccounts.Domain.Tests.Factories
{
    public class BankAccountFactoryTests
    {
        [Fact]
        public void CheckBankAccountDefaultId()
        {
            // Arrange
            Guid id = default;
            InternationalBankAccountNumber iban = default;
            User user = default;
            Movement movement = default;
            var now = new DateTime(2000, 1, 1);

            // Act
            var createFunction = () => BankAccount.Create(id, iban, user, movement, now);

            // Assert
            createFunction
                .Should()
                .Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'id')");
        }

        [Fact]
        public void CheckBankAccountDefaultIban()
        {
            // Arrange
            var id = Guid.NewGuid();
            InternationalBankAccountNumber iban = default;
            User user = default;
            Movement movement = default;
            var now = new DateTime(2000, 1, 1);

            // Act
            var createFunction = () => BankAccount.Create(id, iban, user, movement, now);

            // Assert
            createFunction.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CheckBankAccountDefaultUser()
        {
            // Arrange
            var id = Guid.NewGuid();
            var iban = Substitute.For<InternationalBankAccountNumber>();
            User user = default;
            Movement movement = default;
            var now = new DateTime(2000, 1, 1);

            // Act
            var createFunction = () => BankAccount.Create(id, iban, user, movement, now);

            // Assert
            createFunction.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CheckBankAccountDefaultMovement()
        {
            // Arrange
            var id = Guid.NewGuid();
            var iban = Substitute.For<InternationalBankAccountNumber>();
            var user = Substitute.For<User>();
            Movement movement = default;
            var now = new DateTime(2000, 1, 1);

            // Act
            var createFunction = () => BankAccount.Create(id, iban, user, movement, now);

            // Assert
            createFunction.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CheckBankAccountOk()
        {
            // Arrange
            var id = Guid.NewGuid();
            var iban = Substitute.For<InternationalBankAccountNumber>();
            var user = Substitute.For<User>();
            var movement = Movement.Create(Guid.NewGuid(), "concept", 2, DateTime.Now).Value;
            var now = new DateTime(2000, 1, 1);

            // Act
            var createFunction = () => BankAccount.Create(id, iban, user, movement, now);

            // Assert
            createFunction.Should().NotThrow();
        }

        [Fact]
        public void CheckBankAccountCreatedEvent()
        {
            // Arrange
            var id = Guid.NewGuid();
            var iban = Substitute.For<InternationalBankAccountNumber>();
            var user = UserTestFactory.Create().Value;
            var movement = Movement.Create(Guid.NewGuid(), "concept", 2, DateTime.Now).Value;
            var now = new DateTime(2000, 1, 1);

            // Act
            var bankAccount = BankAccount.Create(id, iban, user, movement, now).Value;

            // Assert
            var events = bankAccount.PullDomainEvents();
            events.Count.Should().BeGreaterOrEqualTo(1);
        }
    }
}