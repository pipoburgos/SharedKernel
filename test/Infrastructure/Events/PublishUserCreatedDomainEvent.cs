using System;
using System.Collections.Generic;

namespace SharedKernel.Integration.Tests.Events
{
    public class PublishUserCreatedDomainEvent
    {
        public PublishUserCreatedDomainEvent()
        {
            Users = new List<Guid>();
        }

        public List<Guid> Users { get; }

        public int Total { get; private set; }

        public void SetUser(Guid id)
        {
            Users.Add(id);
        }

        public void SumTotal()
        {
            Total++;
        }
    }
}
