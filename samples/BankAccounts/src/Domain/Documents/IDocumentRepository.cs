using SharedKernel.Domain.Repositories.Create;
using SharedKernel.Domain.Repositories.Read;

namespace BankAccounts.Domain.Documents;
internal interface IDocumentRepository : ICreateRepositoryAsync<Document>, IReadOneRepository<Document, Guid>
{
}
