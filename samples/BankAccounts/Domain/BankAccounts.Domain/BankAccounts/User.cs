using SharedKernel.Domain.Entities;
using System;

namespace BankAccounts.Domain.BankAccounts
{
    public class User : Entity<Guid>
    {
        protected User() { }

        public User(Guid id, string name, string surname, DateTime dateOfBirth) : base(id)
        {
            Name = name;
            Surname = surname;
            DateOfBirth = dateOfBirth;
        }

        public string Name { get; private set; }

        public DateTime DateOfBirth { get; private set; }

        public string Surname { get; private set; }
    }
}
