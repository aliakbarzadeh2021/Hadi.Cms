using System;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// متدولوؤی های هر دپارتمان
    /// </summary>
    public class Methodology : BaseModel
    {
        public Methodology()
        {
            
        }
        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// شناسه عکس ضمیمه شده
        /// </summary>
        public Guid? ImageAttachmentId { get; set; }
        /// <summary>
        /// شناسه دپارتمان
        /// </summary>
        public Guid DepartmentId { get; set; }

        public Department Department { get; set; }
    }
}
