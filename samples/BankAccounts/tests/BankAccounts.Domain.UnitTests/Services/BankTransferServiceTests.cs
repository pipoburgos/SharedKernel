using BankAccounts.Domain.BankAccounts.Errors;
using BankAccounts.Domain.Services;
using BankAccounts.Domain.Tests.Data;

namespace BankAccounts.Domain.Tests.Services;

public class BankTransferServiceTests
{
    [Fact]
    public void Ok()
    {
        // Arrange
        var fromAccount = BankAccountTestFactory.Create(initialMovement: MovementTestFactory.Create(amount: 50).Value);
        var toAccount = BankAccountTestFactory.Create(initialMovement: MovementTestFactory.Create(amount: 50).Value);
        const int amount = 10;
        var date = new DateTime(1996, 5, 12);
        var fromMovementId = Guid.NewGuid();
        var toMovementId = Guid.NewGuid();

        // Act
        var result =
            new BankTransferService().Transfer(fromAccount, toAccount, amount, date, fromMovementId, toMovementId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        fromAccount.Balance.Should().Be(40);
        toAccount.Balance.Should().Be(60);
    }

    [Fact]
    public void TransferNoMoney()
    {
        // Arrange
        var fromAccount = BankAccountTestFactory.Create(initialMovement: MovementTestFactory.Create(amount: 50).Value);
        var toAccount = BankAccountTestFactory.Create(initialMovement: MovementTestFactory.Create(amount: 50).Value);
        const int amount = 0;
        var date = new DateTime(1996, 5, 12);
        var fromMovementId = Guid.NewGuid();
        var toMovementId = Guid.NewGuid();

        // Act
        var result =
            new BankTransferService().Transfer(fromAccount, toAccount, amount, date, fromMovementId, toMovementId);

        // Assert
        result.Errors.Should().Contain(e => e.ErrorMessage == BankAccountErrors.QuantityCannotBeNegative);
    }
}
