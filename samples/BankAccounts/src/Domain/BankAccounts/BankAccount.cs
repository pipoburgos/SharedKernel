using BankAccounts.Domain.BankAccounts.Events;
using BankAccounts.Domain.BankAccounts.Exceptions;
using BankAccounts.Domain.BankAccounts.Factories;
using BankAccounts.Domain.BankAccounts.Specifications;
using SharedKernel.Domain.Guards;

namespace BankAccounts.Domain.BankAccounts
{
    internal class BankAccount : AggregateRoot<Guid>
    {
        private readonly List<Movement> _movements;

        protected BankAccount()
        {
            _movements = new List<Movement>();
        }

        internal BankAccount(Guid id, InternationalBankAccountNumber internationalBankAccountNumber, User owner,
            Movement initialMovement, DateTime now) : base(id)
        {
            Guard.ThrowIfNull(id);

            Guard.ThrowIfNull(internationalBankAccountNumber);

            Guard.ThrowIfNull(owner);

            Guard.ThrowIfNull(initialMovement);

            if (initialMovement.Amount <= 0)
                throw new QuantityCannotBeNegativeException();

            if (!new AtLeast18YearsOldSpec(now).SatisfiedBy().Compile()(owner))
                throw new AtLeast18YearsOldException();

            InternationalBankAccountNumber = internationalBankAccountNumber;
            Owner = owner;
            _movements = new List<Movement> { initialMovement };
        }

        public InternationalBankAccountNumber InternationalBankAccountNumber { get; private set; }

        public User Owner { get; private set; }

        public decimal Balance => _movements.Sum(m => m.Amount);

        public IEnumerable<Movement> Movements => _movements.AsEnumerable();

        public void WithdrawMoney(Guid id, string concept, decimal quantity, DateTime date)
        {
            if (quantity <= 0)
                throw new QuantityCannotBeNegativeException();

            if (new IsTheBankAccountOverdrawn().SatisfiedBy().Compile()(this))
                throw new OverdraftBankAccountException();

            _movements.Add(MovementFactory.CreateMovement(id, concept, -quantity, date).Value);
        }

        public void MakeDeposit(Guid id, string concept, decimal quantity, DateTime date)
        {
            if (quantity <= 0)
                throw new QuantityCannotBeNegativeException();

            var movement = MovementFactory.CreateMovement(id, concept, quantity, date).Value;
            _movements.Add(movement);

            if (new IsThePayroll().SatisfiedBy().Compile()(movement))
                Record(new SalaryHasBeenDeposited(movement.Id, Id.ToString()));
        }
    }
}
