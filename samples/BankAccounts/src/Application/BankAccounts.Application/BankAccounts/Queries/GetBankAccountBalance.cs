using SharedKernel.Application.Cqrs.Queries;
using System;

namespace BankAccounts.Application.BankAccounts.Queries
{
    /// <summary> Gets bank account balance. </summary>
    public class GetBankAccountBalance : IQueryRequest<decimal>
    {
        /// <summary> Gets bank account balance. </summary>
        public GetBankAccountBalance(Guid bankAccountId, string ownerName)
        {
            BankAccountId = bankAccountId;
            OwnerName = ownerName;
        }

        /// <summary> Bank account identifier. </summary>
        public Guid BankAccountId { get; private set; }

        /// <summary> Contains owner name. </summary>
        public string OwnerName { get; private set; }
    }
}
