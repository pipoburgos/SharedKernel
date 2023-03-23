using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Application.Communication.Email
{
    /// <summary> Email sender manager. </summary>
    public interface IEmailSender
    {
        /// <summary> Sends an email to default email from. </summary>
        /// <returns></returns>
        Task SendEmailAsync(Mail email, CancellationToken cancellationToken);

        /// <summary> Sends emails to default email from. </summary>
        /// <returns></returns>
        Task SendEmailAsync(IEnumerable<Mail> emails, CancellationToken cancellationToken);
    }
}
