using SharedKernel.Application.Cqrs.Queries;
using System;

namespace BankAccounts.Application.BankAccounts.Queries
{
    /// <summary> Gets bank account balance. </summary>
    public class GetBankAccountBalance : IQueryRequest<decimal>
    {
        /// <summary> Constructor. </summary>
        public GetBankAccountBalance(Guid bankAccountId)
        {
            BankAccountId = bankAccountId;
        }

        /// <summary> Bank account identifier. </summary>
        public Guid BankAccountId { get; }
    }
}
