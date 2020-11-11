namespace SharedKernel.Application.Communication.Email
{
    public class EmailAttachment
    {
        public EmailAttachment(string filename, byte[] data)
        {
            Filename = filename;
            Data = data;
        }

        public string Filename { get; }

        public byte[] Data { get; }
    }
}
