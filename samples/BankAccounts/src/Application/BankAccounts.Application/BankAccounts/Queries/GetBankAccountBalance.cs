using SharedKernel.Application.Cqrs.Queries;
using System;

namespace BankAccounts.Application.BankAccounts.Queries
{
    public class GetBankAccountBalance : IQueryRequest<decimal>
    {
        public GetBankAccountBalance(Guid bankAccountId)
        {
            BankAccountId = bankAccountId;
        }

        public Guid BankAccountId { get; }
    }
}
