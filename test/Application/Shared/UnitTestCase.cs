using Moq;
using SharedKernel.Application.System;
using SharedKernel.Domain.Events;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SharedKernel.Application.Tests.Shared
{
    public class UnitTestCase
    {
        protected Mock<IEventBus> EventBus { get; }
        protected Mock<IGuid> UuidGenerator { get; }

        public UnitTestCase()
        {
            EventBus = new Mock<IEventBus>();
            UuidGenerator = new Mock<IGuid>();
        }

        public void ShouldHavePublished(List<DomainEvent> domainEvents)
        {
            EventBus.Verify(x => x.Publish(domainEvents, CancellationToken.None), Times.AtLeastOnce());
        }

        public void ShouldHavePublished(DomainEvent domainEvent)
        {
            ShouldHavePublished(new List<DomainEvent> {domainEvent});
        }

        public void ShouldGenerateUuid(string uuid)
        {
            UuidGenerator.Setup(x => x.NewGuid()).Returns(new Guid(uuid));
        }
    }
}