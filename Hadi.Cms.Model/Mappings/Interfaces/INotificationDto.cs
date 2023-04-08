using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface INotificationDto
    {
        Guid Id { get; set; }
        string Text { get; set; }
        Guid SenderUserId { get; set; }
        Guid? ReceiverUserId { get; set; }
        DateTime SendDateTime { get; set; }
        DateTime? ReadDateTime { get; set; }
        bool Unread { get; set; }
        bool Is { get; set; }
        bool IsDelete { get; set; }
        bool WithNews { get; set; }
        Guid? NewsId { get; set; }
        IUserDto SenderDto { get; set; }
        IUserDto ReceiverDto { get; set; }

    }
}
