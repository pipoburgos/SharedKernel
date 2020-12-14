using System;
using System.Collections.Generic;
using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Tests.Users
{
    internal class User : AggregateRoot<Guid>
    {
        private readonly List<string> _emails;
        private readonly List<Address> _addresses;

        // ReSharper disable once UnusedMember.Local
        private User() { }

        internal User(Guid id, string name, List<string> emails = default, List<Address> addresses = default)
        {
            Id = id;
            Name = name;
            _emails = emails ?? new List<string>();
            _addresses = addresses ?? new List<Address>();
        }

        public static User Create(Guid id, string name)
        {
            var user = new User(id, name);

            user.Record(new UserCreated(id, name, id.ToString()));

            return user;
        }

        public string Name { get; private set; }

        public IEnumerable<string> Emails => _emails;

        public IEnumerable<Address> Addresses => _addresses;

        public User ChangeName(string name)
        {
            Name = name;
            return this;
        }

        public User AddEmail(string email)
        {
            _emails.Add(email);
            return this;
        }

        public User AddAddress(Address address)
        {
            _addresses.Add(address);
            return this;
        }
    }
}
