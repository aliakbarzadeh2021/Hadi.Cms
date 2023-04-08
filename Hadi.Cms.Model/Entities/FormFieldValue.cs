using System;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// مقدار فیلد های فرم
    /// </summary>
    public class FormFieldValue : BaseModel
    {
        public FormFieldValue()
        {
            
        }

        public Guid FormId { get; set; }
        public Guid Code { get; set; }

        /// <summary>
        /// شناسه فیلد
        /// </summary>
        public Guid FormFieldId { get; set; }
        /// <summary>
        /// مقدار
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// ای پی کاربری که این مقدار را به فیلد نسبت داده است
        /// </summary>
        public string IpAddress { get; set; }

        public FormField FormField { get; set; }
    }
}
