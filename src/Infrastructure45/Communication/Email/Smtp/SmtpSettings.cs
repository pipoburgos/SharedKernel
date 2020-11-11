namespace SharedKernel.Infrastructure.Communication.Email.Smtp
{
    public class SmtpSettings
    {
        public string MailServer { get; set; }
        public bool RequireSsl { get; set; }
        public bool RequireTls { get; set; }
        public int MailPort { get; set; }
        public string SenderName { get; set; }
        public string Sender { get; set; }
        public string Password { get; set; }
    }
}
