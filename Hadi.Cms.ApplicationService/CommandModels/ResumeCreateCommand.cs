using System;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ثبت رزومه جدید
    /// </summary>
    public class ResumeCreateCommand
    {
        /// <summary>
        /// شناسه فایل ضمیمه شده
        /// </summary>
        public Guid AttachmentFileId { get; set; }
        /// <summary>
        /// نوع رزومه
        /// </summary>
        public Guid CareerOpportunityId { get; set; }
    }
}
