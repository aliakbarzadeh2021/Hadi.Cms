using Hadi.Cms.Model.Mappings.Interfaces;
using System;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class NotificationDto : INotificationDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid SenderUserId { get; set; }
        public Guid? ReceiverUserId { get; set; }
        public DateTime SendDateTime { get; set; }
        public DateTime? ReadDateTime { get; set; }
        public bool Unread { get; set; }
        public bool Is { get; set; }
        public bool IsDelete { get; set; }
        public bool WithNews { get; set; }
        public Guid? NewsId { get; set; }
        public IUserDto SenderDto { get; set; }
        public IUserDto ReceiverDto { get; set; }
    }
}
