using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// تکنولوژی
    /// </summary>
    public class Technology : BaseModel
    {
        public Technology()
        {
            ProjectTechnologies = new HashSet<ProjectTechnology>();
        }
        /// <summary>
        /// نام
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// لوگو
        /// </summary>
        public Guid? ImageGuid { get; set; }
        /// <summary>
        /// آیا یک ابزار است
        /// </summary>
        public bool IsTool { get; set; }

        public ICollection<ProjectTechnology> ProjectTechnologies { get; set; }
    }
}
