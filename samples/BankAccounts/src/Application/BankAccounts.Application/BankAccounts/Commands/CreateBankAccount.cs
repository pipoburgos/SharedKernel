using SharedKernel.Application.Cqrs.Commands;
using System;

namespace BankAccounts.Application.BankAccounts.Commands
{
    public class CreateBankAccount : ICommandRequest
    {
        public CreateBankAccount(Guid id, Guid ownerId, string name, DateTime birthdate, string surname,
            Guid movementId, decimal amount)
        {
            Id = id;
            OwnerId = ownerId;
            Name = name;
            Birthdate = birthdate;
            Surname = surname;
            MovementId = movementId;
            Amount = amount;
        }

        public Guid Id { get; }

        public Guid OwnerId { get; }

        public string Name { get; }

        public DateTime Birthdate { get; }

        public string Surname { get; }

        public Guid MovementId { get; }

        public decimal Amount { get; }
    }
}
