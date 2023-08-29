namespace SharedKernel.Application.Communication.Email
{
    /// <summary> Email. </summary>
    public class Mail
    {
        /// <summary> . </summary>
        public Mail()
        {
        }

        /// <summary> . </summary>
        public Mail(string to, string subject, string? body = default, string? from = default,
            List<string>? emailsBcc = default, List<MailAttachment>? attachments = default)
        {
            From = from;
            To = new List<string> { to };
            EmailsBcc = emailsBcc;
            Subject = subject;
            Body = body;
            Attachments = attachments;
        }

        /// <summary> . </summary>
        public Mail(List<string> to, string subject, string? body = default, string? from = default,
            List<string>? emailsBcc = default, List<MailAttachment>? attachments = default)
        {
            From = from;
            To = to;
            EmailsBcc = emailsBcc;
            Subject = subject;
            Body = body;
            Attachments = attachments;
        }

        /// <summary> From. </summary>
        public string? From { get; set; }

        /// <summary> To. </summary>
        public List<string> To { get; set; } = null!;

        /// <summary> EmailsBcc. </summary>
        public List<string>? EmailsBcc { get; set; }

        /// <summary> Subject. </summary>
        public string? Subject { get; set; }

        /// <summary> Body. </summary>
        public string? Body { get; set; }

        /// <summary> Attachments. </summary>
        public List<MailAttachment>? Attachments { get; set; }
    }
}