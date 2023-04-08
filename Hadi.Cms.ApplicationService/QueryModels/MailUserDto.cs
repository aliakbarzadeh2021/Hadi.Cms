using Hadi.Cms.Model.Mappings.Interfaces;
using System;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class MailUserDto : IMailUserDto
    {
        public Guid Id { get; set; }
        public Guid MailId { get; set; }
        public Guid SenderUserId { get; set; }
        public Guid ReceiverUserId { get; set; }
        public DateTime SendDateTime { get; set; }
        public bool Unread { get; set; }
        public bool DeletedBySender { get; set; }
        public bool DeletedByReceiver { get; set; }
    }
}
