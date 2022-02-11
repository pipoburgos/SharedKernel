using BankAccounts.Domain.BankAccounts.Exceptions;
using BankAccounts.Domain.Services;
using BankAccounts.Domain.UnitTests.Data;
using FluentAssertions;
using System;
using Xunit;

namespace BankAccounts.Domain.UnitTests.Services
{
    public class BankTransferServiceTests
    {
        [Fact]
        public void Ok()
        {
            // Arrange
            var fromAccount = BankAccountTestFactory.Create(initialMovement: MovementTestFactory.Create(amount: 50));
            var toAccount = BankAccountTestFactory.Create(initialMovement: MovementTestFactory.Create(amount: 50));
            var amount = 10;
            var date = new DateTime(1996, 5, 12);
            var fromMovementId = Guid.NewGuid();
            var toMovementId = Guid.NewGuid();

            // Act
            var result = () => new BankTransferService().Transfer(fromAccount, toAccount, amount, date, fromMovementId, toMovementId);

            // Assert
            result.Should().NotThrow();
            fromAccount.Balance.Should().Be(40);
            toAccount.Balance.Should().Be(60);
        }

        [Fact]
        public void TransferNoMoney()
        {
            // Arrange
            var fromAccount = BankAccountTestFactory.Create(initialMovement: MovementTestFactory.Create(amount: 50));
            var toAccount = BankAccountTestFactory.Create(initialMovement: MovementTestFactory.Create(amount: 50));
            var amount = 0;
            var date = new DateTime(1996, 5, 12);
            var fromMovementId = Guid.NewGuid();
            var toMovementId = Guid.NewGuid();

            // Act
            var result = () => new BankTransferService().Transfer(fromAccount, toAccount, amount, date, fromMovementId, toMovementId);

            // Assert
            result.Should().ThrowExactly<QuantityCannotBeNegativeException>();
        }

        [Fact]
        public void Error()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
