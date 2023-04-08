using Hadi.Cms.Language.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ثبت شبکه اجتماعی
    /// </summary>
    public class SocialNetworkCreateCommand
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string Link { get; set; }
        public bool IsActive { get; set; }
        [AllowHtml]
        public string Source { get; set; }
        public Guid? ImageGuid { get; set; }
    }
}
