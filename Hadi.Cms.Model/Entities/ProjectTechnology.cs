using System;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// تکنولوژی های پروژه
    /// </summary>
    public class ProjectTechnology : BaseModel
    {
        public ProjectTechnology()
        {
            
        }

        public Guid ProjectId { get; set; }
        public Guid TechnologyId { get; set; }
        public Technology Technology { get; set; }
        public Project Project { get; set; }
    }
}
