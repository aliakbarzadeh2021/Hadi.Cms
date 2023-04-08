using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IMailUserDto
    {
        Guid Id { get; set; }
        Guid MailId { get; set; }
        Guid SenderUserId { get; set; }
        Guid ReceiverUserId { get; set; }
        DateTime SendDateTime { get; set; }
        bool Unread { get; set; }
        bool DeletedBySender { get; set; }
        bool DeletedByReceiver { get; set; }
    }
}
