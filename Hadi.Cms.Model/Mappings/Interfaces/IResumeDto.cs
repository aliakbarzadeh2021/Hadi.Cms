using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Enums;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IResumeDto
    {
        /// <summary>
        /// شناسه
        /// </summary>
        Guid Id { get; set; }
        /// <summary>
        /// شناسه فایل ضمیمه شده
        /// </summary>
        Guid? AttachmentFileId { get; set; }
        /// <summary>
        /// منبع فایل ضمیمه شده
        /// </summary>
        string AttachmentFileSource { get; set; }
        /// <summary>
        /// نام فایل ضمیمه شده
        /// </summary>
        string AttachmentFileName { get; set; }
        /// <summary>
        /// نوع فایل ضمیمه شده
        /// </summary>
        string AttachmentFileType { get; set; }
        /// <summary>
        /// تاریخ ایجاد
        /// </summary>
        DateTime CreatedDate { get; set; }
        /// <summary>
        /// نوع ذخیره سازی
        /// </summary>
        string StorageType { get; set; }

        string IpAddress { get; set; }
        bool IsPhysicalStorage { get; set; }
        bool IsBinaryStorage { get; set; }
        string FileSource { get; set; }

        ICareerOpportunityDto CareerOpportunityDto { get; set; }
    }
}
