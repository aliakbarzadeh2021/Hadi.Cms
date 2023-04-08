using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// تگ های دوره
    /// </summary>
    public class CourseTag : BaseModel
    {
        public CourseTag()
        {
            
        }

        public Guid CourseId { get; set; }
        public Guid TagId { get; set; }
        public Course Course { get; set; }
        public Tag Tag { get; set; }
    }
}
