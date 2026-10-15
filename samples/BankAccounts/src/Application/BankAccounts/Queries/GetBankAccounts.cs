namespace BankAccounts.Application.BankAccounts.Queries;

/// <summary> Gets bank account balance. </summary>
public sealed class GetBankAccounts : IQueryRequest<IPagedList<BankAccountItem>>
{
    public GetBankAccounts(PageOptions? pageOptions = null, bool isRobert = false)
    {
        PageOptions = pageOptions;
        IsRobert = isRobert;
    }

    /// <summary> . </summary>
    public PageOptions? PageOptions { get; }

    /// <summary> . </summary>
    public bool IsRobert { get; }
}

/// <summary> . </summary>
public class BankAccountItem
{
    /// <summary> . </summary>
    public Guid Id { get; set; }

    /// <summary> . </summary>
    public string OwnerName { get; set; } = null!;
}
