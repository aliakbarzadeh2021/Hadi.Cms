using System;

namespace Hadi.Cms.Model.Entities
{
    public class MailUser
    {
        public MailUser()
        {
            Id = Guid.NewGuid();
            SendDateTime = DateTime.Now;
        }

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
