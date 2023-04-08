using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hadi.Cms.Model.Entities
{
    public class Notification
    {
        public Notification()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid SenderUserId { get; set; }
        public Guid? ReceiverUserId { get; set; }
        public DateTime SendDateTime { get; set; }
        public DateTime? ReadDateTime { get; set; }
        public bool Unread { get; set; }
        public bool IsPublic { get; set; }
        public bool IsDelete { get; set; }
        public bool WithNews { get; set; }
        public Guid? NewsId { get; set; }

        [ForeignKey("SenderUserId")]
        public virtual User Sender { get; set; }

        [ForeignKey("ReceiverUserId")]
        public virtual User Receiver { get; set; }
    }
}
