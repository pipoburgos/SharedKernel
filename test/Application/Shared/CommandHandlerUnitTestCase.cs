using Moq;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Repositories;

namespace SharedKernel.Application.Tests.Shared
{
    public abstract class CommandHandlerUnitTestCase<TRepository, TAggregateRoot> : UnitTestCase
        where TRepository : class, IRepositoryAsync<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public MockRepositoryAsync<TRepository, TAggregateRoot> Repository { get; }

        protected CommandHandlerUnitTestCase()
        {
            Repository = new MockRepositoryAsync<TRepository, TAggregateRoot>();
        }

        protected void ShouldHaveSave()
        {
            Repository.Verify(x => x.Save(), Times.AtLeastOnce());
        }
    }
}