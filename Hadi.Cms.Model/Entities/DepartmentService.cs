using System;

namespace Hadi.Cms.Model.Entities
{
    public class DepartmentService : BaseModel
    {
        public DepartmentService()
        {
            
        }

        public Guid DepartmentId { get; set; }
        public Guid ServiceId { get; set; }
        public Department Department { get; set; }
        public Service Service { get; set; }
    }
}
