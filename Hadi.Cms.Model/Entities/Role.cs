
using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Entities
{
    public class Role
    {
        public Role()
        {
            Id = Guid.NewGuid();
            UserRoles = new HashSet<UserRole>();
            RoleFeatures = new HashSet<RoleFeature>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<RoleFeature> RoleFeatures { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
