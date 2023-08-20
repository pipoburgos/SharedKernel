namespace SharedKernel.Infrastructure.Communication.Email.Smtp
{
    /// <summary>  </summary>
    public class SmtpSettings
    {
        /// <summary>  </summary>
        public long? MaxSendSize { get; set; }

        /// <summary>  </summary>
        public string MailServer { get; set; }

        /// <summary>  </summary>
        public bool RequireSsl { get; set; }

        /// <summary>  </summary>
        public bool RequireTls { get; set; }

        /// <summary>  </summary>
        public int MailPort { get; set; }

        /// <summary>  </summary>
        public string DefaultSender { get; set; }

        /// <summary>  </summary>
        public string User { get; set; }

        /// <summary>  </summary>
        public string Password { get; set; }
    }
}
