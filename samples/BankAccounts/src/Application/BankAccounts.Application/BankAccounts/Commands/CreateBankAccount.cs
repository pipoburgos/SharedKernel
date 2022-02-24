using SharedKernel.Application.Cqrs.Commands;
using System;

namespace BankAccounts.Application.BankAccounts.Commands
{
    /// <summary> Create a bank account. </summary>
    public class CreateBankAccount : ICommandRequest
    {
        /// <summary> Constructor. </summary>
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

        /// <summary> Bank account identifier. </summary>
        public Guid Id { get; }

        /// <summary> Owner identifier. </summary>
        public Guid OwnerId { get; }

        /// <summary> Owner name. </summary>
        public string Name { get; }

        /// <summary> Owner surname. </summary>
        public string Surname { get; }

        /// <summary> Owner birthdate. </summary>
        public DateTime Birthdate { get; }

        /// <summary> Movement identifier. </summary>
        public Guid MovementId { get; }

        /// <summary> Initial amount. </summary>
        public decimal Amount { get; }
    }
}
