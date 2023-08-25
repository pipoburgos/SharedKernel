using BankAccounts.Application.BankAccounts.Queries;
using BankAccounts.Domain.BankAccounts;
using BankAccounts.Domain.BankAccounts.Repository;

namespace BankAccounts.Infrastructure.BankAccounts.Queries
{
    internal class GetBankAccountBalanceHandler : IQueryRequestHandler<GetBankAccountBalance, decimal>
    {
        //private readonly DapperQueryProvider _queryProvider;
        private readonly IBankAccountRepository _bankAccountRepository;

        public GetBankAccountBalanceHandler(
            //DapperQueryProvider queryProvider,
            IBankAccountRepository bankAccountRepository)
        {
            //_queryProvider = queryProvider;
            _bankAccountRepository = bankAccountRepository;
        }

        public async Task<decimal> Handle(GetBankAccountBalance query, CancellationToken cancellationToken)
        {
            //return _queryProvider.ExecuteQueryFirstOrDefaultAsync<decimal>("", cancellationToken);
            var bankAccount = await _bankAccountRepository
                .GetByIdAsync(BankAccountId.Create(query.BankAccountId), cancellationToken);

            return bankAccount.Balance;
        }
    }
}
