using System;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// برچسب های پروژه
    /// </summary>
    public class ProjectTag : BaseModel
    {
        public ProjectTag()
        {
            
        }
        /// <summary>
        /// شناسه پروژه
        /// </summary>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// شناسه برچسب
        /// </summary>
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
        public Project Project { get; set; }
    }
}
