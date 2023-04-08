using Hadi.Cms.Model.Entities;
using Hadi.Cms.Repository.UnitOfWork;
using System;
using System.IO;
using System.Web;

namespace Hadi.Cms.Configuration
{
    public static class Settings
    {
        private static string _blobStoragePath = null;
        private static string _physicalStoragePath = null;
        private static int? _usersDataVersion = null;
        private static string _applicationCompanyName = null;
        private static bool? _applicationLoginWithSms = null;
        private static bool? _applicationLoginWithEmail = null;
        private static bool? _applicationLoginWithCaptcha = null;
        private static bool? _applicationPhysicalStorage = null;
        private static bool? _applicationBinaryStorage = null;
        private static bool? _applicationUserHasDepartment = null;
        private static bool? _applicationEnableIpBanned = null;
        private static bool? _applicationEnableIpRange = null;
        private static bool? _applicationFailedLoginLimitedCheck = null;
        private static int? _applicationFailedLoginMaximumCount = null;
        private static bool? _applicationDisplayFooterVersionText = null;
        private static bool? _applicationDisplayFooterLanguageChanger = null;
        private static bool? _applicationDisplayNotificationTopIcon = null;
        private static bool? _applicationDisplaySiteLeftMenuBar = null;
        private static bool? _applicationDisplayViewLogo = null;
        private static bool? _applicationDisplayLoginLogo = null;
        private static bool? _quartzActivated = null;
        private static string _applicationViewLogoPath = null;
        private static string _applicationLoginLogoPath = null;
        private static string _applicationTextUnderLoginLogo = null;
        private static string _applicationPanelFooterText = null;
        private static string _applicationViewFooterText = null;
        private static string _applicationLoginFooterText = null;
        private static string _applicationLicense = null;
        private static string _applicationLanguage = null;
        private static string _applicationENamadCode = null;


        public static string BlobStoragePath
        {
            get
            {
                if (_blobStoragePath == null)
                {
                    _blobStoragePath = GetValue("BlobStoragePath", Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "/PhysicalStorage"));
                }

                return _blobStoragePath;
            }
            set
            {
                var propertyValue = value == null ? "" : value.ToString();
                SaveValue("BlobStoragePath", propertyValue);
                _blobStoragePath = propertyValue;
            }
        }

        public static string PhysicalStoragePath
        {
            get
            {
                if (_physicalStoragePath == null)
                {
                    _physicalStoragePath = GetValue("PhysicalStoragePath", HttpContext.Current.Server.MapPath("/PhysicalStorage"));
                }

                return _physicalStoragePath;
            }
            set
            {
                var propertyValue = value == null ? "" : value.ToString();
                SaveValue("PhysicalStoragePath", propertyValue);
                _physicalStoragePath = propertyValue;
            }
        }

        public static int UsersDataVersion
        {
            get
            {
                if (_usersDataVersion == null)
                {
                    _usersDataVersion = int.Parse(GetValue("UsersDataVersion", "0"));
                }

                return _usersDataVersion ?? 0;
            }
            set
            {
                SaveValue("UsersDataVersion", value.ToString());
                _usersDataVersion = value;
            }
        }

        public static string Application_CompanyName
        {
            get
            {
                if (_applicationCompanyName == null)
                {
                    _applicationCompanyName = GetValue("Application_CompanyName", "هادی");
                }

                return _applicationCompanyName;
            }
            set
            {
                var propertyValue = value == null ? "" : value.ToString();
                SaveValue("Application_CompanyName", propertyValue);
                _applicationCompanyName = propertyValue;
            }
        }

        public static string Application_Language
        {
            get
            {
                if (_applicationLanguage == null)
                {
                    _applicationLanguage = GetValue("Application_Language", "fa-ir");
                }

                return _applicationLanguage;
            }
            set
            {
                var propertyValue = value == null ? "" : value.ToString();
                SaveValue("Application_Language", propertyValue);
                _applicationLanguage = propertyValue;
            }
        }

        public static bool Application_LoginWithSms
        {
            get
            {
                if (_applicationLoginWithSms == null)
                {
                    _applicationLoginWithSms = bool.Parse(GetValue("Application_LoginWithSms", "false"));
                }

                return _applicationLoginWithSms ?? false;
            }
            set
            {
                SaveValue("Application_LoginWithSms", value.ToString());
                _applicationLoginWithSms = value;
            }
        }

        public static bool Application_LoginWithEmail
        {
            get
            {
                if (_applicationLoginWithEmail == null)
                {
                    _applicationLoginWithEmail = bool.Parse(GetValue("Application_LoginWithEmail", "false"));
                }

                return _applicationLoginWithEmail ?? false;
            }
            set
            {
                SaveValue("Application_LoginWithEmail", value.ToString());
                _applicationLoginWithEmail = value;
            }
        }

        public static bool Application_LoginWithCaptcha
        {
            get
            {
                if (_applicationLoginWithCaptcha == null)
                {
                    _applicationLoginWithCaptcha = bool.Parse(GetValue("Application_LoginWithCaptcha", "false"));
                }

                return _applicationLoginWithCaptcha ?? false;
            }
            set
            {
                SaveValue("Application_LoginWithCaptcha", value.ToString());
                _applicationLoginWithCaptcha = value;
            }
        }

        public static bool Application_PhysicalStorage
        {
            get
            {
                if (_applicationPhysicalStorage == null)
                {
                    _applicationPhysicalStorage = bool.Parse(GetValue("Application_PhysicalStorage", "true"));
                }

                return _applicationPhysicalStorage ?? false;
            }
            set
            {
                SaveValue("Application_PhysicalStorage", value.ToString());
                _applicationPhysicalStorage = value;
            }
        }

        public static bool Application_BinaryStorage
        {
            get
            {
                if (_applicationBinaryStorage == null)
                {
                    _applicationBinaryStorage = bool.Parse(GetValue("Application_BinaryStorage", "false"));
                }

                return _applicationBinaryStorage ?? false;
            }
            set
            {
                SaveValue("Application_BinaryStorage", value.ToString());
                _applicationBinaryStorage = value;
            }
        }

        public static bool Application_UserHasDepartment
        {
            get
            {
                if (_applicationUserHasDepartment == null)
                {
                    _applicationUserHasDepartment = bool.Parse(GetValue("Application_UserHasDepartment", "false"));
                }

                return _applicationUserHasDepartment ?? false;
            }
            set
            {
                SaveValue("Application_UserHasDepartment", value.ToString());
                _applicationUserHasDepartment = value;
            }
        }

        public static bool Application_EnableIpBanned
        {
            get
            {
                if (_applicationEnableIpBanned == null)
                {
                    _applicationEnableIpBanned = bool.Parse(GetValue("Application_EnableIpBanned", "false"));
                }

                return _applicationEnableIpBanned ?? false;
            }
            set
            {
                SaveValue("Application_EnableIpBanned", value.ToString());
                _applicationEnableIpBanned = value;
            }
        }

        public static bool Application_EnableIpRange
        {
            get
            {
                if (_applicationEnableIpRange == null)
                {
                    _applicationEnableIpRange = bool.Parse(GetValue("Application_EnableIpRange", "false"));
                }

                return _applicationEnableIpRange ?? false;
            }
            set
            {
                SaveValue("Application_EnableIpRange", value.ToString());
                _applicationEnableIpRange = value;
            }
        }

        public static bool Application_FailedLoginLimitedCheck
        {
            get
            {
                if (_applicationFailedLoginLimitedCheck == null)
                {
                    _applicationFailedLoginLimitedCheck = bool.Parse(GetValue("Application_FailedLoginLimitedCheck", "false"));
                }

                return _applicationFailedLoginLimitedCheck ?? false;
            }
            set
            {
                SaveValue("Application_FailedLoginLimitedCheck", value.ToString());
                _applicationFailedLoginLimitedCheck = value;
            }
        }

        public static bool? QuartzActivated
        {

            get
            {
                return _quartzActivated ?? false;
            }
            set
            {
                _quartzActivated = value;
            }
        }

        public static int Application_FailedLoginMaximumCount
        {
            get
            {
                if (_applicationFailedLoginMaximumCount == null)
                {
                    _applicationFailedLoginMaximumCount = int.Parse(GetValue("Application_FailedLoginMaximumCount", "5"));
                }

                return _applicationFailedLoginMaximumCount ?? 0;
            }
            set
            {
                SaveValue("Application_FailedLoginMaximumCount", value.ToString());
                _applicationFailedLoginMaximumCount = value;
            }
        }

        public static bool Application_DisplayFooterVersionText
        {
            get
            {
                if (_applicationDisplayFooterVersionText == null)
                {
                    _applicationDisplayFooterVersionText = bool.Parse(GetValue("Application_DisplayFooterVersionText", "false"));
                }

                return _applicationDisplayFooterVersionText ?? false;
            }
            set
            {
                SaveValue("Application_DisplayFooterVersionText", value.ToString());
                _applicationDisplayFooterVersionText = value;
            }
        }

        public static bool Application_DisplayFooterLanguageChanger
        {
            get
            {
                if (_applicationDisplayFooterLanguageChanger == null)
                {
                    _applicationDisplayFooterLanguageChanger = bool.Parse(GetValue("Application_DisplayFooterLanguageChanger", "false"));
                }

                return _applicationDisplayFooterLanguageChanger ?? false;
            }
            set
            {
                SaveValue("Application_DisplayFooterLanguageChanger", value.ToString());
                _applicationDisplayFooterLanguageChanger = value;
            }
        }

        public static bool Application_DisplayNotificationTopIcon
        {
            get
            {
                if (_applicationDisplayNotificationTopIcon == null)
                {
                    _applicationDisplayNotificationTopIcon = bool.Parse(GetValue("Application_DisplayNotificationTopIcon", "false"));
                }

                return _applicationDisplayNotificationTopIcon ?? false;
            }
            set
            {
                SaveValue("Application_DisplayNotificationTopIcon", value.ToString());
                _applicationDisplayNotificationTopIcon = value;
            }
        }

        public static bool Application_DisplaySiteLeftMenuBar
        {
            get
            {
                if (_applicationDisplaySiteLeftMenuBar == null)
                {
                    _applicationDisplaySiteLeftMenuBar = bool.Parse(GetValue("Application_DisplaySiteLeftMenuBar", "false"));
                }

                return _applicationDisplaySiteLeftMenuBar ?? false;
            }
            set
            {
                SaveValue("Application_DisplaySiteLeftMenuBar", value.ToString());
                _applicationDisplaySiteLeftMenuBar = value;
            }
        }

        public static bool Application_DisplayViewLogo
        {
            get
            {
                if (_applicationDisplayViewLogo == null)
                {
                    _applicationDisplayViewLogo = bool.Parse(GetValue("Application_DisplayViewLogo", "false"));
                }

                return _applicationDisplayViewLogo ?? false;
            }
            set
            {
                SaveValue("Application_DisplayViewLogo", value.ToString());
                _applicationDisplayViewLogo = value;
            }
        }

        public static bool Application_DisplayLoginLogo
        {
            get
            {
                if (_applicationDisplayLoginLogo == null)
                {
                    _applicationDisplayLoginLogo = bool.Parse(GetValue("Application_DisplayLoginLogo", "false"));
                }

                return _applicationDisplayLoginLogo ?? false;
            }
            set
            {
                SaveValue("Application_DisplayLoginLogo", value.ToString());
                _applicationDisplayLoginLogo = value;
            }
        }

        public static string Application_ViewLogoPath
        {
            get
            {
                if (_applicationViewLogoPath == null)
                {
                    _applicationViewLogoPath = GetValue("Application_ViewLogoPath", "~/Content/Images/favicon.ico");
                }

                return _applicationViewLogoPath;
            }
            set
            {
                var propertyValue = value == null ? "" : value.ToString();
                SaveValue("Application_ViewLogoPath", propertyValue);
                _applicationViewLogoPath = propertyValue;
            }
        }

        public static string Application_LoginLogoPath
        {
            get
            {
                if (_applicationLoginLogoPath == null)
                {
                    _applicationLoginLogoPath = GetValue("Application_LoginLogoPath", "~/Content/Images/Hadi_Logo.png");
                }

                return _applicationLoginLogoPath;
            }
            set
            {
                var propertyValue = value == null ? "" : value.ToString();
                SaveValue("Application_LoginLogoPath", propertyValue);
                _applicationLoginLogoPath = propertyValue;
            }
        }

        public static string Application_TextUnderLoginLogo
        {
            get
            {
                if (_applicationTextUnderLoginLogo == null)
                {
                    _applicationTextUnderLoginLogo = GetValue("Application_TextUnderLoginLogo", " ");
                }

                return _applicationTextUnderLoginLogo;
            }
            set
            {
                var propertyValue = value == null ? "" : value.ToString();
                SaveValue("Application_TextUnderLoginLogo", propertyValue);
                _applicationTextUnderLoginLogo = propertyValue;
            }
        }

        public static string Application_PanelFooterText
        {
            get
            {
                if (_applicationPanelFooterText == null)
                {
                    _applicationPanelFooterText = GetValue("Application_PanelFooterText", "");
                }

                return _applicationPanelFooterText;
            }
            set
            {
                var propertyValue = value == null ? "" : value.ToString();
                SaveValue("Application_PanelFooterText", propertyValue);
                _applicationPanelFooterText = propertyValue;
            }
        }
        public static string Application_ViewFooterText
        {
            get
            {
                if (_applicationViewFooterText == null)
                {
                    _applicationViewFooterText = GetValue("Application_ViewFooterText", "");
                }

                return _applicationViewFooterText;
            }
            set
            {
                var propertyValue = value == null ? "" : value.ToString();
                SaveValue("Application_ViewFooterText", propertyValue);
                _applicationViewFooterText = propertyValue;
            }
        }

        public static string Application_LoginFooterText
        {
            get
            {
                if (_applicationLoginFooterText == null)
                {
                    _applicationLoginFooterText = GetValue("Application_LoginFooterText", "");
                }

                return _applicationLoginFooterText;
            }
            set
            {
                var propertyValue = value == null ? "" : value.ToString();
                SaveValue("Application_LoginFooterText", propertyValue);
                _applicationLoginFooterText = propertyValue;
            }
        }

        public static string Application_License
        {
            get
            {
                if (_applicationLicense == null)
                {
                    _applicationLicense = GetValue("Application_License", "EAAAAD4NOASLkb2GU7IOoLPKd7h/kdqkxxPRnOAW9yiifjeSZTwIqvA9+pnyaaof+vS6CQ==");
                }

                return _applicationLicense;
            }
            set
            {
                var propertyValue = value == null ? "" : value.ToString();
                SaveValue("Application_License", propertyValue);
                _applicationLicense = propertyValue;
            }
        }

        public static string Application_ENamadCode
        {
            get
            {
                if (_applicationENamadCode == null)
                {
                    _applicationENamadCode = GetValue("Application_ENamadCode",
                        "<a class='e-namad' referrerpolicy='origin' target='_blank' href='https://trustseal.enamad.ir/?id=178212&amp;Code=F5xwHpa3K0PVqPAWJmCk'><img referrerpolicy='origin'src = 'https://Trustseal.eNamad.ir/logo.aspx?id=178212&amp;Code=F5xwHpa3K0PVqPAWJmCk' alt ='' style='cursor:pointer' id ='F5xwHpa3K0PVqPAWJmCk'></a>");
                }

                return _applicationENamadCode;
            }
            set
            {
                var propertyValue = value == null ? "" : value.ToString();
                SaveValue("Application_ENamadCode", propertyValue);
                _applicationENamadCode = propertyValue;
            }
        }

        #region Internal

        private static void SaveValue(string key, string value)
        {
            using (DataContext dataContext = new DataContext())
            {
                var setting = dataContext.SettingRepository.Get(q => q.Key == key);

                if (setting != null)
                    setting.Value = value;
                else
                {
                    setting = new Setting()
                    {
                        Id = Guid.NewGuid(),
                        Key = key,
                        Value = value
                    };
                    dataContext.SettingRepository.Insert(setting);
                }
                dataContext.Save();
            }

        }

        private static string GetValue(string key, string defaultValue = null)
        {
            using (DataContext dataContext = new DataContext())
            {
                var setting = dataContext.SettingRepository.Get(q => q.Key == key);
                if (setting != null)
                    return setting.Value;

                return defaultValue;
            }
        }

        #endregion
    }

}
