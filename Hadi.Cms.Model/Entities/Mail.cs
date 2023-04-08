
using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Entities
{
    public class Mail
    {
        public Mail()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public virtual ICollection<MailAttachment> MailAttachments { get; set; }
    }
}
