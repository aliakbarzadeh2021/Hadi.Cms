using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IRoleDto
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string DisplayName { get; set; }
        bool IsActive { get; set; }
        ICollection<IRoleFeatureDto> RoleFeaturesDto { get; set; }
        ICollection<IUserRoleDto> UserRolesDto { get; set; }
    }
}
