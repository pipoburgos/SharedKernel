using BankAccounts.Domain.BankAccounts.Events;
using BankAccounts.Domain.BankAccounts.Repository;
using SharedKernel.Application.Communication.Email;
using SharedKernel.Application.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

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

            await _emailSender.SendEmailAsync(bankAccount.Owner.Name, "Bank account created");
        }
    }
}
