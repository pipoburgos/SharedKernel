namespace SharedKernel.Application.Communication.Email
{
    /// <summary> An email attachment. </summary>
    public class MailAttachment
    {
        /// <summary> An email attachment. </summary>
        /// <param name="filename"></param>
        /// <param name="contentStream"></param>
        public MailAttachment(string filename, Stream contentStream)
        {
            Filename = filename;
            ContentStream = contentStream;
        }

        /// <summary> Filename with extension. </summary>
        public string Filename { get; }

        /// <summary> Content stream. </summary>
        public Stream ContentStream { get; set; }
    }
}
