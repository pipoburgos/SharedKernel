using BankAccounts.Application.BankAccounts.Queries;
using BankAccounts.Domain.BankAccounts;
using SharedKernel.Infrastructure.Data.Queryable;


namespace BankAccounts.Infrastructure.BankAccounts.Queries;

internal sealed class GetBankAccountsHandler : IQueryRequestHandler<GetBankAccounts, IPagedList<BankAccountItem>>
{
    private readonly EntityFrameworkCoreQueryProvider<BankAccountDbContext> _queryProvider;

    public GetBankAccountsHandler(
        EntityFrameworkCoreQueryProvider<BankAccountDbContext> queryProvider)
    {
        _queryProvider = queryProvider;
    }

    public Task<IPagedList<BankAccountItem>> Handle(GetBankAccounts query, CancellationToken cancellationToken)
    {
        return _queryProvider
            .GetQuery<BankAccount>()
            .Where(query.IsRobert, x => x.Owner.Name == "Robert")
            .Select(x => new BankAccountItem
            {
                Id = x.Id.Value,
                OwnerName = x.Owner.Name,
            })
            .ToPagedListAsync(query.PageOptions!, cancellationToken);
    }
}