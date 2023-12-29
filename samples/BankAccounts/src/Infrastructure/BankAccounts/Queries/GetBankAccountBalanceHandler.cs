using BankAccounts.Application.BankAccounts.Queries;
using BankAccounts.Domain.BankAccounts;
using BankAccounts.Domain.BankAccounts.Repository;

namespace BankAccounts.Infrastructure.BankAccounts.Queries;

internal sealed class GetBankAccountBalanceHandler : IQueryRequestHandler<GetBankAccountBalance, decimal>
{
    private readonly IBankAccountRepository _bankAccountRepository;

    public GetBankAccountBalanceHandler(IBankAccountRepository bankAccountRepository)
    {
        _bankAccountRepository = bankAccountRepository;
    }

    public async Task<decimal> Handle(GetBankAccountBalance query, CancellationToken cancellationToken)
    {
        var bankAccount = await _bankAccountRepository
            .GetByIdAsync(BankAccountId.Create(query.BankAccountId), cancellationToken);

        if (bankAccount == default!)
            return await Task.FromResult(0);

        return bankAccount.Balance;
    }
}
