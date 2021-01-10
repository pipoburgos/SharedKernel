using System;

namespace SharedKernel.Infraestructure.Tests.Events
{
    public class PublishUserCreatedDomainEvent
    {
        public Guid UserId { get; private set; }

        public int Total { get; private set; }

        public void SetUser(Guid id)
        {
            UserId = id;
            SumTotal();
        }

        public void SumTotal()
        {
            Total++;
        }
    }
}
