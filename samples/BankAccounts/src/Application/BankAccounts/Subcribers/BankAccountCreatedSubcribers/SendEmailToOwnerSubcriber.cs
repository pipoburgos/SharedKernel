using BankAccounts.Domain.BankAccounts;
using BankAccounts.Domain.BankAccounts.Events;
using BankAccounts.Domain.BankAccounts.Repository;
using SharedKernel.Application.Communication.Email;

namespace BankAccounts.Application.BankAccounts.Subcribers.BankAccountCreatedSubcribers
{
    internal class SendEmailToOwnerSubcriber : IDomainEventSubscriber<BankAccountCreated>
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

        public async Task On(BankAccountCreated @event, CancellationToken cancellationToken)
        {
            var bankAccountId = BankAccountId.Create(new Guid(@event.AggregateId));
            var bankAccount = await _bankAccountRepository.GetByIdAsync(bankAccountId, cancellationToken);

            if (bankAccount == default!)
                return;

            var email = new Mail("a@a.es", "Bank account created", bankAccount.InternationalBankAccountNumber.ToString());
            await _emailSender.SendEmailAsync(email, cancellationToken);
        }
    }
}
