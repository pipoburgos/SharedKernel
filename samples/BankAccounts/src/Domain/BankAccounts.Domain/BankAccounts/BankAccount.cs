using SharedKernel.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankAccounts.Domain.BankAccounts
{
    public class BankAccount : AggregateRoot<Guid>
    {
        private readonly List<Movement> _movements;

        protected BankAccount()
        {
            _movements = new List<Movement>();
        }

        public BankAccount(Guid id, InternationalBankAccountNumber internationalBankAccountNumber, User owner,
            Movement initialMovement) : base(id)
        {
            if (id == default)
                throw new ArgumentNullException(nameof(id));

            if (internationalBankAccountNumber == default)
                throw new ArgumentNullException(nameof(internationalBankAccountNumber));

            if (owner == default)
                throw new ArgumentNullException(nameof(owner));

            if (initialMovement == default)
                throw new ArgumentNullException(nameof(initialMovement));

            InternationalBankAccountNumber = internationalBankAccountNumber;
            Owner = owner;
            _movements = new List<Movement> { initialMovement };
        }

        public InternationalBankAccountNumber InternationalBankAccountNumber { get; private set; }

        public User Owner { get; private set; }

        public decimal Balance => _movements.Sum(m => m.Amount);

        public IEnumerable<Movement> Movements => _movements.AsReadOnly();
    }
}
