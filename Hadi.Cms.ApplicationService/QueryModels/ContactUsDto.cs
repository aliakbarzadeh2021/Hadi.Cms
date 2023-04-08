using Hadi.Cms.Language.Resources;
using Hadi.Cms.Model.Mappings.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class ContactUsDto : IContactUsDto
    {
        public Guid Id { get; set; }
        public DateTime? CreatedWhen { get; set; }
        public string UserIp { get; set; }

        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "ContactUsModel_UsernameRequired")]
        [Display(ResourceType = typeof(Strings), Name = "ContactUsModel_Username")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "ContactUsModel_UserEmailRequired")]
        [Display(ResourceType = typeof(Strings), Name = "ContactUsModel_UserEmail")]
        public string UserEmail { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "ContactUsModel_UserMobile")]
        public string UserMobile { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "ContactUsModel_Subject")]
        public string Subject { get; set; }

        [Required(ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "ContactUsModel_TextRequired")]
        [Display(ResourceType = typeof(Strings), Name = "ContactUsModel_Text")]
        public string Text { get; set; }
    }
}
