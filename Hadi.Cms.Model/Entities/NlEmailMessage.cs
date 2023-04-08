using System;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// ایمیل های ارسال شده به ایمیل های موجود در سامانه
    /// </summary>
    public class NlMessageEmail : BaseModel
    {
        public NlMessageEmail()
        {

        }

        public Guid NlEmailId { get; set; }
        public Guid NlMessageId { get; set; }
        public bool Published { get; set; }
        public NlEmail NlEmail { get; set; }
        public NlMessage NlMessage { get; set; }
    }
}
