using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// ایمیل های موجود در سامانه برای دریافت خبرنامه
    /// </summary>
    public class NlEmail : BaseModel
    {
        public NlEmail()
        {
            NlMessageEmails = new HashSet<NlMessageEmail>();
        }

        public string Email { get; set; }
        public ICollection<NlMessageEmail> NlMessageEmails { get; set; }
    }
}
