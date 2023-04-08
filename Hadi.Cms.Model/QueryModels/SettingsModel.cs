using Hadi.Cms.Language.Resources;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Hadi.Cms.Model.QueryModels
{
    public class SettingsModel
    {
        [Display(ResourceType = typeof(Strings), Name = "Application_CompanyName")]
        public string Application_CompanyName { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Application_LoginWithSms")]
        public bool Application_LoginWithSms { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Application_LoginWithEmail")]
        public bool Application_LoginWithEmail { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Application_LoginWithCaptcha")]
        public bool Application_LoginWithCaptcha { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Application_BinaryStorage")]
        public bool Application_BinaryStorage { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Application_PhysicalStorage")]
        public bool Application_PhysicalStorage { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Application_UserHasDepartment")]
        public bool Application_UserHasDepartment { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Application_EnableIpBanned")]
        public bool Application_EnableIpBanned { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Application_EnableIpRange")]
        public bool Application_EnableIpRange { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Application_FailedLoginLimitedCheck")]
        public bool Application_FailedLoginLimitedCheck { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Application_FailedLoginMaximumCount")]
        public int Application_FailedLoginMaximumCount { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Application_DisplayFooterVersionText")]
        public bool Application_DisplayFooterVersionText { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Application_DisplayFooterLanguageChanger")]
        public bool Application_DisplayFooterLanguageChanger { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Application_DisplayNotificationTopIcon")]
        public bool Application_DisplayNotificationTopIcon { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Application_DisplayLeftMenuBar")]
        public bool Application_DisplaySiteLeftMenuBar { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Application_DisplayViewLogo")]
        public bool Application_DisplayViewLogo { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Application_DisplayLoginLogo")]
        public bool Application_DisplayLoginLogo { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Application_ViewLogoPath")]
        public string Application_ViewLogoPath { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Application_LoginLogoPath")]
        public string Application_LoginLogoPath { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Application_TextUnderLoginLogo")]
        public string Application_TextUnderLoginLogo { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Application_PanelFooterText")]
        public string Application_PanelFooterText { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Application_ViewFooterText")]
        public string Application_ViewFooterText { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Application_LoginFooterText")]
        public string Application_LoginFooterText { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Application_License")]
        public string Application_License { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Application_Language")]
        [Required(AllowEmptyStrings = false , ErrorMessageResourceType = typeof(Strings),ErrorMessageResourceName = "Required")]
        public string Application_Language { get; set; }
        [Display(ResourceType = typeof(Strings), Name = "Application_ENamadCode")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Application_ENamadCode { get; set; }
    }
}
