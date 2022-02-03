using SharedKernel.Domain.Entities;
using System;

namespace BankAccounts.Domain.BankAccounts
{
    public class User : Entity<Guid>
    {
        protected User() { }

        public User(Guid id, string name, string surname, DateTime birthdate) : base(id)
        {
            Name = name;
            Surname = surname;
            Birthdate = birthdate;
        }

        public string Name { get; private set; }

        public DateTime Birthdate { get; private set; }

        public string Surname { get; private set; }
    }
}
