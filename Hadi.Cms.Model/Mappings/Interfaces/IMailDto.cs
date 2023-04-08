using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IMailDto
    {
        Guid Id { get; set; }
        string Title { get; set; }
        string Text { get; set; }
        IUserDto Sender { get; set; }
        List<IUserDto> Receivers { get; set; }
        DateTime? SendTime { get; set; }
        bool Unread { get; set; }
        bool WithAttachment { get; set; }
        List<IAttachmentFileDto> Attachments { get; set; }
        ICollection<IMailAttachmentDto> MailAttachmentsDto { get; set; }
    }
}
