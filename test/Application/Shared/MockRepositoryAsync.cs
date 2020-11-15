using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Repositories;

namespace SharedKernel.Application.Tests.Shared
{
    internal class MockRepositoryAsync<TRepository, TAggregateRoot> : Mock<TRepository>
        where TRepository : class, IRepositoryAsync<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {

        public void MockGetByIdAsync(TAggregateRoot result)
        {
            Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None)).Returns(Task.FromResult(result));
        }

        public void VerifyGetByIdAsync(Times times)
        {
            Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None), times);
        }

        public void VerifySaveAsync()
        {
            Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.AtLeastOnce());
        }
    }
}
