using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// کارفرما
    /// </summary>
    public class Employer : BaseModel
    {
        public Employer()
        {
            Projects = new HashSet<Project>();   
        }

        /// <summary>
        /// شناسه لوگوی کارفرما
        /// </summary>
        public Guid LogoGuid { get; set; }
        /// <summary>
        /// نام کارفرما
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// نام مدیرعامل
        /// </summary>
        public string CEOName { get; set; }
        /// <summary>
        /// تلفن
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// آدرس
        /// </summary>
        public string Address { get; set; }
        public ICollection<Project> Projects { get; set; }
    }
}
