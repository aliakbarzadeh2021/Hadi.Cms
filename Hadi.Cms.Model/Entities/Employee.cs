using System;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// کارمندان
    /// </summary>
    public class Employee : BaseModel
    {
        public Employee()
        {
            
        }
        /// <summary>
        /// شناسه دپارتمان
        /// </summary>
        public Guid DepartmentId { get; set; }
        /// <summary>
        /// نام
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// نام خانوادگی
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// شناسه عکس ضمیمه شده
        /// </summary>
        public Guid? AttachmentImageId { get; set; }
        /// <summary>
        /// عنوان نقش کارمند
        /// </summary>
        public string RoleTitle { get; set; }
        /// <summary>
        /// اولویت
        /// </summary>
        public int OrderId { get; set; }

        public Department Department { get; set; }
    }
}
