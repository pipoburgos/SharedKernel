using BankAccounts.Domain.BankAccounts;
using BankAccounts.Domain.BankAccounts.Repository;

namespace BankAccounts.Application.BankAccounts.Commands;

internal class CreateBankAccountHandler : ICommandRequestHandler<CreateBankAccount, ApplicationResult<ApplicationUnit>>
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

    public Task<ApplicationResult<ApplicationUnit>> Handle(CreateBankAccount command,
        CancellationToken cancellationToken) =>
        Result
            .Create(InternationalBankAccountNumber.Create("ES14", "1234", "12", "32", "0123456789"))
            .Combine(
                User.Create(command.OwnerId, command.Name, command.Surname, command.Birthdate),
                Movement.Create(command.MovementId, "Initial movement", command.Amount, _dateTime.UtcNow))
            .Bind(t => BankAccount.Create(BankAccountId.Create(command.Id), t.Item1.Value, t.Item2, t.Item3,
                _dateTime.UtcNow))
            .Tap(bankAccount => _bankAccountRepository.AddAsync(bankAccount, cancellationToken))
            .Tap(_ => _unitOfWork.SaveChangesAsync(cancellationToken))
            .Tap(bankAccount => _eventBus.Publish(bankAccount.PullDomainEvents(), cancellationToken))
            .ToApplicationResultUnit();
}
