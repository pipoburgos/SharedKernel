using SharedKernel.Application.Communication.Email;

namespace BankAccounts.Acceptance.Tests.Shared.Mocks
{
    public class EmailTestSender : IEmailSender
    {
        public Task SendEmailAsync(Mail email, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task SendEmailAsync(IEnumerable<Mail> emails, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}