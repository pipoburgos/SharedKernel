using BankAccounts.Domain.BankAccounts.Errors;
using BankAccounts.Domain.BankAccounts.Events;
using BankAccounts.Domain.BankAccounts.Specifications;

namespace BankAccounts.Domain.BankAccounts
{
    internal class BankAccount : AggregateRoot<BankAccountId>
    {
        private readonly List<Movement> _movements;

        protected BankAccount()
        {
            _movements = new();
        }

        protected BankAccount(BankAccountId id, InternationalBankAccountNumber internationalBankAccountNumber, User owner,
            Movement initialMovement) : base(id)
        {
            Guard.ThrowIfNullOrDefault(id);
            Guard.ThrowIfNullOrDefault(internationalBankAccountNumber);
            Guard.ThrowIfNullOrDefault(owner);
            Guard.ThrowIfNullOrDefault(initialMovement);
            InternationalBankAccountNumber = internationalBankAccountNumber;
            Owner = owner;
            _movements = new List<Movement> { initialMovement };
        }

        public static Result<BankAccount> Create(BankAccountId id, InternationalBankAccountNumber accountNumber, User owner,
            Movement initialMovement, DateTime now) =>
            Result
                .Create(owner)
                .Ensure(o => o == default! || new AtLeast18YearsOldSpec(now).SatisfiedBy().Compile()(o),
                    Error.Create(BankAccountErrors.AtLeast18YearsOld, nameof(owner)))
                .EnsureAppendError(_ => initialMovement == default! || initialMovement.Amount > 0,
                    Error.Create(BankAccountErrors.QuantityCannotBeNegative, nameof(initialMovement)))
                .Map(_ => new BankAccount(id, accountNumber, owner, initialMovement))
                .Tap(bankAccount => bankAccount.Record(new BankAccountCreated(bankAccount.Id.Value.ToString()!)));

        public InternationalBankAccountNumber InternationalBankAccountNumber { get; private set; } = null!;

        public User Owner { get; private set; } = null!;

        public decimal Balance => _movements.Sum(m => m.Amount);

        public IEnumerable<Movement> Movements => _movements.AsEnumerable();

        public Result<Unit> WithdrawMoney(Guid id, string concept, decimal quantity, DateTime date) =>
            Result
                .Create(Unit.Value)
                .Ensure(_ => quantity > 0, Error.Create(BankAccountErrors.QuantityCannotBeNegative, nameof(quantity)))
                .EnsureAppendError(_ => !new IsTheBankAccountOverdrawn().SatisfiedBy().Compile()(this),
                    Error.Create(BankAccountErrors.OverdraftBankAccount))
                .Tap(_ => _movements.Add(Movement.Create(id, concept, -quantity, date).Value));

        public Result<Unit> MakeDeposit(Guid id, string concept, decimal quantity, DateTime date)
        {
            var movement = Result
                .Create(Unit.Value)
                .Ensure(_ => quantity > 0, Error.Create(BankAccountErrors.QuantityCannotBeNegative, nameof(quantity)))
                .Bind(_ => Movement.Create(id, concept, quantity, date))
                .Tap(movement => _movements.Add(movement));

            if (movement.IsSuccess && new IsThePayroll().SatisfiedBy().Compile()(movement.Value))
                Record(new SalaryHasBeenDeposited(movement.Value.Id, Id.ToString()!));

            return Result.Success();
        }
    }
}
