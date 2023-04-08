using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// فایل های ضمیمه شده ی دوره
    /// </summary>
    public class CourseAttachmentFile : BaseModel
    {
        public CourseAttachmentFile()
        {
            
        }

        /// <summary>
        /// شناسه دوره
        /// </summary>
        public Guid CourseId { get; set; }
        /// <summary>
        /// شناسه فایل ضمیمه شده
        /// </summary>
        public Guid AttachmentFileId { get; set; }
        public Course Course { get; set; }
        public AttachmentFile AttachmentFile { get; set; }
    }
}
