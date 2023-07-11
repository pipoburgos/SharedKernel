using SharedKernel.Application.Cqrs.Queries.Contracts;
using SharedKernel.Application.Cqrs.Queries.Entities;

namespace BankAccounts.Application.BankAccounts.Queries;

/// <summary> Gets bank account balance. </summary>
public class GetBankAccounts : IQueryRequest<IPagedList<BankAccountItem>>
{
    /// <summary>  </summary>
    public PageOptions? PageOptions { get; set; }
}

/// <summary>  </summary>
public class BankAccountItem
{
    /// <summary>  </summary>
    public Guid Id { get; set; }
}
