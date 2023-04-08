using Hadi.Cms.Language.Resources;
using Hadi.Cms.Model.Mappings.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class RoleDto : IRoleDto
    {
        public Guid Id { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "RoleModel_RoleName")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "RoleModel_RoleDisplayName")]
        public string DisplayName { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "RoleModel_IsActive")]
        public bool IsActive { get; set; }
        public ICollection<IRoleFeatureDto> RoleFeaturesDto { get; set; }
        public ICollection<IUserRoleDto> UserRolesDto { get; set; }
    }
}
