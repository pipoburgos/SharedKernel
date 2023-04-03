using BankAccounts.Application.Shared.UnitOfWork;
using BankAccounts.Domain.BankAccounts;
using BankAccounts.Domain.BankAccounts.Factories;
using BankAccounts.Domain.BankAccounts.Repository;

namespace BankAccounts.Application.BankAccounts.Commands
{
    internal class CreateBankAccountHandler : ICommandRequestHandler<CreateBankAccount>
    {
        private readonly IDateTime _dateTime;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IBankAccountUnitOfWork _unitOfWork;
        private readonly IEventBus _eventBus;

        public CreateBankAccountHandler(
            IDateTime dateTime,
            IBankAccountRepository bankAccountRepository,
            IBankAccountUnitOfWork unitOfWork,
            IEventBus eventBus)
        {
            _dateTime = dateTime;
            _bankAccountRepository = bankAccountRepository;
            _unitOfWork = unitOfWork;
            _eventBus = eventBus;
        }

        public async Task Handle(CreateBankAccount command, CancellationToken cancellationToken)
        {
            var iban = new InternationalBankAccountNumber("ES14", "1234", "12", "32", "1234123412341");

            var owner = UserFactory.CreateUser(command.OwnerId, command.Name, command.Surname, command.Birthdate);

            var movement = MovementFactory.CreateMovement(command.MovementId, "Initial movement", command.Amount,
                _dateTime.UtcNow);

            var bankAccount = BankAccountFactory.Create(command.Id, iban, owner, movement, _dateTime.UtcNow);

            await _bankAccountRepository.AddAsync(bankAccount, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _eventBus.Publish(bankAccount.PullDomainEvents(), cancellationToken);
        }
    }
}
