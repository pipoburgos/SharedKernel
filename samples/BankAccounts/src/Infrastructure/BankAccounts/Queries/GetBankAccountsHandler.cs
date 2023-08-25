using BankAccounts.Application.BankAccounts.Queries;
using BankAccounts.Domain.BankAccounts;


namespace BankAccounts.Infrastructure.BankAccounts.Queries
{
    internal class GetBankAccountsHandler : IQueryRequestHandler<GetBankAccounts, IPagedList<BankAccountItem>>
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
                .Select(x => new BankAccountItem
                {
                    Id = x.Id.Value
                })
                .ToPagedListAsync(query.PageOptions!, cancellationToken);
        }
    }
}
