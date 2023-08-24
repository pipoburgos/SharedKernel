using BankAccounts.Application.BankAccounts.Queries;
using BankAccounts.Domain.BankAccounts;
using BankAccounts.Infrastructure.Shared.Data;
using SharedKernel.Application.Cqrs.Queries.Contracts;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.Extensions;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.Queries;

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
                    Id = x.Id
                })
                .ToPagedListAsync(query.PageOptions!, cancellationToken);
        }
    }
}
