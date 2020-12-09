using System;

namespace SharedKernel.Integration.Tests.Events
{
    public class PublishUserCreatedDomainEvent
    {
        public Guid UserId { get; private set; }

        public int Total { get; private set; }

        public void SetUser(Guid id)
        {
            UserId = id;
            Total++;
        }

        public void SumTotal()
        {
            Total++;
        }
    }
}
