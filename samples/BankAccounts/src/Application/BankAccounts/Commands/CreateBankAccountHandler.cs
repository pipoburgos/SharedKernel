using BankAccounts.Domain.BankAccounts;
using BankAccounts.Domain.BankAccounts.Repository;
using BankAccounts.Domain.Documents;

namespace BankAccounts.Application.BankAccounts.Commands;

internal sealed class CreateBankAccountHandler : ICommandRequestHandler<CreateBankAccount, Result<Unit>>
{
    private readonly IDateTime _dateTime;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IDocumentRepository _documentRepository;
    private readonly IBankAccountUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;

    public CreateBankAccountHandler(
        IDateTime dateTime,
        IBankAccountRepository bankAccountRepository,
        IDocumentRepository documentRepository,
        IBankAccountUnitOfWork unitOfWork,
        IEventBus eventBus)
    {
        _dateTime = dateTime;
        _bankAccountRepository = bankAccountRepository;
        _documentRepository = documentRepository;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
    }

    public Task<Result<Unit>> Handle(CreateBankAccount command,
        CancellationToken cancellationToken) =>
        Result
            .Create(InternationalBankAccountNumber.Create("ES14", "1234", "12", "32", "0123456789"))
            .Combine(
                User.Create(command.OwnerId, command.Name, command.Surname, command.Birthdate),
                Movement.Create(command.MovementId, "Initial movement", command.Amount, _dateTime.UtcNow))
            .Bind(t => BankAccount.Create(BankAccountId.Create(command.BankAccountId), t.Item1.Value, t.Item2, t.Item3,
                _dateTime.UtcNow))
            .Tap(bankAccount => _bankAccountRepository.AddAsync(bankAccount, cancellationToken))
            .Tap(bankAccount => _documentRepository
                .AddAsync(Document.Create(bankAccount.Id.Value, "Some text"), cancellationToken))
            .Tap(_ => _unitOfWork.SaveChangesResultAsync(cancellationToken))
            .Tap(bankAccount => _eventBus.Publish(bankAccount.PullDomainEvents(), cancellationToken))
            .Map(_ => Unit.Value);
}
