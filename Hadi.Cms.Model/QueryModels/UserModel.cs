using System;
using System.ComponentModel.DataAnnotations;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.Model.QueryModels
{
    public class UserModel
    {
        [Display(ResourceType = typeof(Strings), Name = "UserName")]
        [Required(AllowEmptyStrings = false , ErrorMessageResourceType = typeof(Strings),ErrorMessageResourceName = "Required")]
        public string UserName { get; set; }
        
        [Display(ResourceType = typeof(Strings), Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string Password { get; set; }
        
        [Display(ResourceType = typeof(Strings), Name = "RetypePassword")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        [Compare("Password",ErrorMessageResourceType = typeof(Strings),ErrorMessageResourceName = "User_Passowrd_Not_Match_With_Confirm")]
        public string RetypePassword { get; set; }
        
        [Display(ResourceType = typeof(Strings), Name = "FirstName")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string FirstName { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "LastName")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string LastName { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Language")]
        public Guid LanguageId { get; set; }
        [Display(ResourceType = typeof(Strings), Name = "IsEnabled")]
        public bool IsEnabled { get; set; } = false;
    }
}
