using System;

namespace Hadi.Cms.Model.Entities
{
    public class MailAttachment
    {
        public MailAttachment()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid MailId { get; set; }
        public Guid AttachmentId { get; set; }
        public AttachmentFile AttachmentFile { get; set; }
        public Mail Mail { get; set; }
    }
}
