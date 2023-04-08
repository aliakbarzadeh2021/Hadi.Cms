using System;
using System.ComponentModel.DataAnnotations;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ثبت متدولوژی
    /// </summary>
    public class MethodologyCreateCommand
    {
        /// <summary>
        /// عنوان
        /// </summary>
        [Required(AllowEmptyStrings = false , ErrorMessageResourceType = typeof(Strings),ErrorMessageResourceName = "Required")]
        public string Title { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        /// <summary>
        /// شناسه عکس ضمیمه شده
        /// </summary>
        public Guid? ImageAttachmentId { get; set; }
        /// <summary>
        /// شناسه دپارتمان
        /// </summary>
        public Guid DepartmentId { get; set; }
        /// <summary>
        /// فعال
        /// </summary>
        public bool IsActive { get; set; }
    }
}
