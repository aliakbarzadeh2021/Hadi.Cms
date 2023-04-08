using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// نظرات در مورد خدمت
    /// </summary>
    public class ServiceComment : BaseModel
    {
        public ServiceComment()
        {

        }

        /// <summary>
        /// شناسه خدمت
        /// </summary>
        public Guid ServiceId { get; set; }
        /// <summary>
        /// متن نظر
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// نام کامل نظر دهنده
        /// </summary>
        public string PersonFullName { get; set; }
        /// <summary>
        /// نقش نظر دهنده
        /// </summary>
        public string PersonRoleTitle { get; set; }

        public Guid? AttachmentImageId { get; set; }
        public Service Service { get; set; }
    }
}
