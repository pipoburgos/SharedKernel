namespace BankAccounts.Application.BankAccounts.Commands;

/// <summary>
/// Create a bank account.
/// </summary>
/// <param name="OwnerId"> Owner identifier.</param>
/// <param name="Name"> Owner name. </param>
/// <param name="Birthdate"> Owner birthdate. </param>
/// <param name="MovementId"> Movement identifier. </param>
/// <param name="Amount"> Initial amount.</param>
/// /// <param name="Surname">  Owner surname.</param>
public sealed record CreateBankAccount(Guid OwnerId, string Name, DateTime Birthdate,
    Guid MovementId, decimal Amount, string? Surname = null) : ICommandRequest<Result<Unit>>
{
    internal Guid BankAccountId { get; private init; }

    /// <summary> Adds bank account identifier </summary>
    /// <param name="bankAccountId"></param>
    public CreateBankAccount WithBankAccountId(Guid bankAccountId) => this with { BankAccountId = bankAccountId };
}