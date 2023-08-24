using SharedKernel.Application.Communication.Email;

namespace SharedKernel.Integration.Tests.Communication.Email
{
    internal class EmailTestFactory
    {
        public static Mail Create(MailAttachment attachment = default)
        {
            return new Mail("robertofernandez1983@gmail.com", "subject", "body", default, default,
                attachment != default ? new List<MailAttachment> { attachment } : default);
        }
    }
}
