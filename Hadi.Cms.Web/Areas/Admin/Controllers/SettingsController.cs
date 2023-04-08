using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Configuration;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Model.QueryModels;
using Hadi.Cms.Web.Controllers;
using Hadi.Cms.Web.Utilities;
using System.Web.Mvc;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    public class SettingsController : BaseController
    {
        private UserService _userService;
        private LanguageService _languageService;

        public SettingsController()
        {
            _userService = new UserService();
            _languageService = new LanguageService();
        }

        [HttpGet]
        public ActionResult Index()
        {
            SettingsModel settingsModel = new SettingsModel();

            settingsModel.Application_CompanyName = Settings.Application_CompanyName;
            settingsModel.Application_LoginWithSms = Settings.Application_LoginWithSms;
            settingsModel.Application_LoginWithEmail = Settings.Application_LoginWithEmail;
            settingsModel.Application_LoginWithCaptcha = Settings.Application_LoginWithCaptcha;
            settingsModel.Application_BinaryStorage = Settings.Application_BinaryStorage;
            settingsModel.Application_PhysicalStorage = Settings.Application_PhysicalStorage;
            settingsModel.Application_UserHasDepartment = Settings.Application_UserHasDepartment;
            settingsModel.Application_EnableIpBanned = Settings.Application_EnableIpBanned;
            settingsModel.Application_EnableIpRange = Settings.Application_EnableIpRange;
            settingsModel.Application_FailedLoginLimitedCheck = Settings.Application_FailedLoginLimitedCheck;
            settingsModel.Application_FailedLoginMaximumCount = Settings.Application_FailedLoginMaximumCount;
            settingsModel.Application_DisplayFooterVersionText = Settings.Application_DisplayFooterVersionText;
            settingsModel.Application_DisplayFooterLanguageChanger = Settings.Application_DisplayFooterLanguageChanger;
            settingsModel.Application_DisplayNotificationTopIcon = Settings.Application_DisplayNotificationTopIcon;
            settingsModel.Application_DisplaySiteLeftMenuBar = Settings.Application_DisplaySiteLeftMenuBar;
            settingsModel.Application_DisplayViewLogo = Settings.Application_DisplayViewLogo;
            settingsModel.Application_DisplayLoginLogo = Settings.Application_DisplayLoginLogo;
            settingsModel.Application_ViewLogoPath = Settings.Application_ViewLogoPath;
            settingsModel.Application_LoginLogoPath = Settings.Application_LoginLogoPath;
            settingsModel.Application_TextUnderLoginLogo = Settings.Application_TextUnderLoginLogo;
            settingsModel.Application_PanelFooterText = Settings.Application_PanelFooterText;
            settingsModel.Application_ViewFooterText = Settings.Application_ViewFooterText;
            settingsModel.Application_LoginFooterText = Settings.Application_LoginFooterText;
            settingsModel.Application_License = Settings.Application_License;
            settingsModel.Application_Language = Settings.Application_Language;
            settingsModel.Application_ENamadCode = Settings.Application_ENamadCode;

            return View(settingsModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(SettingsModel settingsModel)
        {
            if (ModelState.IsValid)
            {
                Settings.Application_CompanyName = settingsModel.Application_CompanyName;
                Settings.Application_LoginWithSms = settingsModel.Application_LoginWithSms;
                Settings.Application_LoginWithEmail = settingsModel.Application_LoginWithEmail;
                Settings.Application_LoginWithCaptcha = settingsModel.Application_LoginWithCaptcha;
                Settings.Application_BinaryStorage = settingsModel.Application_BinaryStorage;
                Settings.Application_PhysicalStorage = settingsModel.Application_PhysicalStorage;
                Settings.Application_UserHasDepartment = settingsModel.Application_UserHasDepartment;
                Settings.Application_EnableIpBanned = settingsModel.Application_EnableIpBanned;
                Settings.Application_EnableIpRange = settingsModel.Application_EnableIpRange;
                Settings.Application_FailedLoginLimitedCheck = settingsModel.Application_FailedLoginLimitedCheck;
                Settings.Application_FailedLoginMaximumCount = settingsModel.Application_FailedLoginMaximumCount;
                Settings.Application_DisplayFooterVersionText = settingsModel.Application_DisplayFooterVersionText;
                Settings.Application_DisplayFooterLanguageChanger = settingsModel.Application_DisplayFooterLanguageChanger;
                Settings.Application_DisplayNotificationTopIcon = settingsModel.Application_DisplayNotificationTopIcon;
                Settings.Application_DisplaySiteLeftMenuBar = settingsModel.Application_DisplaySiteLeftMenuBar;
                Settings.Application_DisplayViewLogo = settingsModel.Application_DisplayViewLogo;
                Settings.Application_DisplayLoginLogo = settingsModel.Application_DisplayLoginLogo;
                Settings.Application_ViewLogoPath = settingsModel.Application_ViewLogoPath;
                Settings.Application_LoginLogoPath = settingsModel.Application_LoginLogoPath;
                Settings.Application_TextUnderLoginLogo = settingsModel.Application_TextUnderLoginLogo;
                Settings.Application_PanelFooterText = settingsModel.Application_PanelFooterText;
                Settings.Application_ViewFooterText = settingsModel.Application_ViewFooterText;
                Settings.Application_LoginFooterText = settingsModel.Application_LoginFooterText;
                Settings.Application_License = settingsModel.Application_License;
                Settings.Application_Language = settingsModel.Application_Language;
                Settings.Application_ENamadCode = settingsModel.Application_ENamadCode;

                var culture = new System.Globalization.CultureInfo(settingsModel.Application_Language);
                var strCultureCode = culture.LCID.ToString();
                var language = _languageService.Get(q => q.CultureCode == strCultureCode);
                SessionData.ChangeCurrentLanguage(culture.Name);

                var user = _userService.GetById(SessionData.Current.User.Id).MaptoEntity();
                user.LanguageId = language.Id;
                _userService.Update(user);
                _userService.Save();

                ViewBag.Success = "1";
                return RedirectToAction("Index");
            }

            ViewBag.Success = "2";
            return View("Index", settingsModel);
        }
    }
}