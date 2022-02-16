using BankAccounts.Application.BankAccounts.Queries;
using BankAccounts.Domain.BankAccounts.Repository;
using SharedKernel.Infrastructure.Cqrs.Queries;
using System.Threading;
using System.Threading.Tasks;

namespace BankAccounts.Infrastructure.BankAccounts.Queries
{
    internal class GetBankAccountBalanceHandler : IQueryRequestHandler<GetBankAccountBalance, decimal>
    {
        private readonly IBankAccountRepository _bankAccountRepository;

        public GetBankAccountBalanceHandler(
            IBankAccountRepository bankAccountRepository)
        {
            _bankAccountRepository = bankAccountRepository;
        }

        public async Task<decimal> Handle(GetBankAccountBalance query, CancellationToken cancellationToken)
        {
            var bankAccount = await _bankAccountRepository.GetByIdAsync(query.BankAccountId, cancellationToken);

            return bankAccount.Balance;
        }
    }
}
