using BankAccounts.Domain.BankAccounts.Events;
using BankAccounts.Domain.BankAccounts.Repository;
using SharedKernel.Application.Communication.Email;

namespace BankAccounts.Application.BankAccounts.Subcribers.BankAccountCreatedSubcribers
{
    internal class SendEmailToOwnerSubcriber : DomainEventSubscriber<BankAccountCreated>
    {
        private readonly IEmailSender _emailSender;
        private readonly IBankAccountRepository _bankAccountRepository;

        public SendEmailToOwnerSubcriber(
            IEmailSender emailSender,
            IBankAccountRepository bankAccountRepository)
        {
            _emailSender = emailSender;
            _bankAccountRepository = bankAccountRepository;
        }

        protected override async Task On(BankAccountCreated @event, CancellationToken cancellationToken)
        {
            var bankAccountId = new Guid(@event.AggregateId);
            var bankAccount = await _bankAccountRepository.GetByIdAsync(bankAccountId, cancellationToken);

            var email = new Mail(bankAccount.Owner.Name, "Bank account created", "subject");
            await _emailSender.SendEmailAsync(email, cancellationToken);
        }
    }
}
