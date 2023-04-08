using Hadi.Cms.Language.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    public class SocialNetworkEditCommand
    {
        public Guid Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string Link { get; set; }
        public bool IsActive { get; set; }
        [AllowHtml]
        public string Source { get; set; }
        public string ImageSource { get; set; }
        public Guid? ImageGuid { get; set; }
    }
}
