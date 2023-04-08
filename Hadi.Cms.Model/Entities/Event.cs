using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// رویداد
    /// </summary>
    public class Event : BaseModel
    {
        public Event()
        {
            
        }

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// لینک
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// شناسه تصویر
        /// </summary>
        public Guid? AttachmentImageId { get; set; }
    }
}
