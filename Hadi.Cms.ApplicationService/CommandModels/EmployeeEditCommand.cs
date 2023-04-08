using Hadi.Cms.Language.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ویرایش کارمند
    /// </summary>
    public class EmployeeEditCommand
    {
        public Guid Id { get; set; }
        /// <summary>
        /// شناسه دپارتمان
        /// </summary>
        public Guid DepartmentId { get; set; }
        /// <summary>
        /// نام
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string FirstName { get; set; }
        /// <summary>
        /// نام خانوادگی
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string LastName { get; set; }
        /// <summary>
        /// شناسه عکس ضمیمه شده
        /// </summary>
        public Guid? AttachmentImageId { get; set; }

        public string ImageSource { get; set; }

        /// <summary>
        /// عنوان نقش کارمند
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string RoleTitle { get; set; }
        /// <summary>
        /// اولویت
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// فعال
        /// </summary>
        public bool IsActive { get; set; }
    }
}
