using Hadi.Cms.Language.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace Hadi.Cms.Model.QueryModels
{
    public class RoleModel
    {
        public Guid Id { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "RoleModel_RoleName")]
        [Required(AllowEmptyStrings = false , ErrorMessageResourceType = typeof(Strings),ErrorMessageResourceName = "Required")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "RoleModel_RoleDisplayName")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string DisplayName { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "RoleModel_IsActive")]
        public bool IsActive { get; set; }
    }
}
