using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Entities
{
    public class Feature
    {
        public Feature()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string FeaturesName { get; set; }
        public string AreaName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Attributes { get; set; }
        public virtual ICollection<RoleFeature> RoleFeatures { get; set; }
    }
}
