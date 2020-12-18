namespace SharedKernel.Application.Communication.Email
{
    /// <summary>
    /// An email attachment
    /// </summary>
    public class EmailAttachment
    {
        /// <summary>
        /// An email attachment
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="data"></param>
        public EmailAttachment(string filename, byte[] data)
        {
            Filename = filename;
            Data = data;
        }

        /// <summary>
        /// Filename with extension
        /// </summary>
        public string Filename { get; }

        /// <summary>
        /// Attachment contents
        /// </summary>
        public byte[] Data { get; }
    }
}
