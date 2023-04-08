using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IMailAttachmentDto
    {
        Guid Id { get; set; }
        Guid MailId { get; set; }
        Guid AttachmentId { get; set; }
    }
}
