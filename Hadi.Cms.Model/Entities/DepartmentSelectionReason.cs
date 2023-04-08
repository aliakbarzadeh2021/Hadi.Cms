using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// دلیل انتخاب دپارتمان
    /// </summary>
    public class DepartmentSelectionReason : BaseModel
    {
        public DepartmentSelectionReason()
        {
            
        }

        public Guid DepartmentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid? AttachmentImageId { get; set; }
        public Department Department { get; set; }
    }
}
