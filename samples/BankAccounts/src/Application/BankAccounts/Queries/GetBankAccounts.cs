namespace BankAccounts.Application.BankAccounts.Queries;

/// <summary> Gets bank account balance. </summary>
public sealed class GetBankAccounts : IQueryRequest<IPagedList<BankAccountItem>>
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
