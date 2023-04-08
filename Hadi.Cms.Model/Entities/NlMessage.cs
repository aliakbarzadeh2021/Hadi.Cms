using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// خبرهایی که قرار است به ایمیل های موجود در سامانه ارسال شود
    /// </summary>
    public class NlMessage : BaseModel
    {
        public NlMessage()
        {
            NlMessageEmails = new HashSet<NlMessageEmail>();
        }
        public string Title { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public Guid? AttachmentId { get; set; }
        public ICollection<NlMessageEmail> NlMessageEmails { get; set; }
    }
}
