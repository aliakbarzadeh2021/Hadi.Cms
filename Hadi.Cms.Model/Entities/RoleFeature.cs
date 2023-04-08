
using System;

namespace Hadi.Cms.Model.Entities
{
    public class RoleFeature
    {
        public RoleFeature()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public Guid FeatureId { get; set; }
        public virtual Feature Feature { get; set; }
        public virtual Role Role { get; set; }
    }
}
