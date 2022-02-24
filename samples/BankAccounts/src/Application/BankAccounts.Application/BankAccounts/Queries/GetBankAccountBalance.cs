using SharedKernel.Application.Cqrs.Queries;
using System;

namespace BankAccounts.Application.BankAccounts.Queries
{
    /// <summary> Gets bank account balance. </summary>
    public class GetBankAccountBalance : IQueryRequest<decimal>
    {
        /// <summary> Constructor. </summary>
        /// <param name="ownerName"></param>
        public GetBankAccountBalance(string ownerName)
        {
            OwnerName = ownerName;
        }

        /// <summary> Bank account identifier. </summary>
        public Guid BankAccountId { get; private set; }

        /// <summary> Contains owner name. </summary>
        public string OwnerName { get; }

        /// <summary> Add bank account identifier. </summary>
        public Guid AddId(Guid id) => BankAccountId = id;
    }
}
