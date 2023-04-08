using Hadi.Cms.Language.Resources;
using System.ComponentModel.DataAnnotations;

namespace Hadi.Cms.Model.QueryModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "LoginViewModel_UsernameRequired")]
        [Display(ResourceType = typeof(Strings), Name = "LoginViewModel_Username")]
        public string Username { get; set; }

        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "LoginViewModel_PasswordRequired")]
        [Display(ResourceType = typeof(Strings), Name = "LoginViewModel_Password")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
