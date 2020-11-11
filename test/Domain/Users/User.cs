using System;
using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Tests.Users
{
    internal class User : AggregateRoot<Guid>
    {
        public static User Create(Guid id, string name)
        {
            var user = new User
            {
                Id = id,
                Name = name
            };

            user.Record(new UserCreated(id, name, id.ToString()));

            return user;
        }

        public string Name { get; set; }
    }
}
