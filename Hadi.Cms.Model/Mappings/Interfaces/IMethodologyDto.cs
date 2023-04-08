using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IMethodologyDto
    {

        Guid Id { get; set; }
        /// <summary>
        /// عنوان
        /// </summary>
        string Title { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        string Description { get; set; }
        /// <summary>
        /// شناسه عکس ضمیمه شده
        /// </summary>
        Guid? ImageAttachmentId { get; set; }
        /// <summary>
        /// منبع عکس
        /// </summary>
        string ImageSource { get; set; }
        /// <summary>
        /// شناسه دپارتمان
        /// </summary>
        Guid DepartmentId { get; set; }

        Guid CreatedBy { get; set; }
        /// <summary>
        /// فعال
        /// </summary>
        bool IsActive { get; set; }

        DateTime CreatedDate { get; set; }
        IDepartmentDto DepartmentDto { get; set; }
    }
}
