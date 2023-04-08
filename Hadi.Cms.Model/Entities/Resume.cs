using System;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// رزومه
    /// </summary>
    public class Resume : BaseModel
    {
        public Resume()
        {
            
        }
        /// <summary>
        /// شناسه شغل
        /// </summary>
        public Guid CareerOpportunityId { get; set; }
        /// <summary>
        /// شناسه فایل ضمیمه شده
        /// </summary>
        public Guid AttachmentFileId { get; set; }
        /// <summary>
        /// ای پی کاربری که رزومه آپلود کرده است
        /// </summary>
        public string IpAddress { get; set; }
        public CareerOpportunity CareerOpportunity { get; set; }
    }
}
