namespace BilligKwhWebApp.Core.Domain
{
    public class EmailAttachment : BaseEntity
    {
        // Properties
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        // Foreign Keys
        public int MessageId { get; set; }

        // Ctor
        public EmailAttachment() { }
        public EmailAttachment(string fileName, byte[] fileContent)
        {
            FileName = fileName;
            FileContent = fileContent;
        }
        public EmailAttachment(string fileName, byte[] fileContent, int messageId)
        {
            FileName = fileName;
            FileContent = fileContent;
            MessageId = messageId;
        }

        // Commands
        public EmailAttachment SetMessageId(int messageId)
        {
            MessageId = messageId;
            return this;
        }
    }
}
