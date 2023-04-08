using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Configuration;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Repository.UnitOfWork;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web;

namespace Hadi.Cms.Web.Utilities
{
    public class SessionData : HttpSessionStateBase
    {
        private DataContext _dataContext;
        private UserService _userService;

        private static string SessionDataKey = "hadiCMS_SESSION_DATA";
        private Guid m_userId;
        private static string culture = "";

        private SessionData()
        {
            _dataContext = new DataContext();
            _userService = new UserService();
        }

        public User User
        {
            get
            {
                var user = _userService.Get(u => u.Id == m_userId, u => u.UserProfiles, u => u.LoginHistories, u => u.UserContacts).MaptoEntity();
                return user;
            }
        }

        public bool CurrentUserIsAuthenticate
        {
            get
            {
                var user = _userService.Get(u => u.Id == m_userId);
                if (user == null)
                    return false;
                else
                    return true;
            }
        }

        public bool CurrentUserIsAdministrator
        {
            get
            {
                var user = _dataContext.UserRepository.Get(u => u.Id == m_userId);
                if (user == null)
                    return false;
                else
                {
                    if (user.UserRoles.Any(u => u.Role.Name.ToLower() == "globaladministrator"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public bool HasRightToLeftUI
        {
            get
            {
                //return Thread.CurrentThread.CurrentUICulture.TextInfo.IsRightToLeft;
                return this.Culture.TextInfo.IsRightToLeft;
            }
        }

        public CultureInfo Culture
        {
            get
            {
                if (!string.IsNullOrEmpty(culture))
                {
                    return new CultureInfo(culture);
                }

                return Infrastructure.Utilities.Utility.GetCultureFromCookie();
            }
        }

        public static SessionData Current
        {
            get
            {
                if (HttpContext.Current.Session == null)
                    return null;

                if (HttpContext.Current.Session[SessionDataKey] as SessionData == null)
                    HttpContext.Current.Session[SessionDataKey] = new SessionData();

                return HttpContext.Current.Session[SessionDataKey] as SessionData;
            }
        }

        public static void CreateSessionDataFor(User user)
        {
            using (DataContext dataContext = new DataContext())
            {
                var currentSessionData = new SessionData();
                currentSessionData.m_userId = user.Id;
                SessionData.culture = user?.Language?.CultureName;

                // log in loginHistory table
                var newLog = new LoginHistory()
                {
                    CreateDate = DateTime.Now,
                    IsLogin = true,
                    UserId = user.Id
                };
                dataContext.LoginHistoryRepository.Insert(newLog);

                // set and check failed login limit
                if (Settings.Application_FailedLoginLimitedCheck)
                    user.LoginRetryCount = 0;

                dataContext.Save();

                if (user.Language != null && !string.IsNullOrEmpty(user.Language.CultureName))
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(user.Language.CultureName);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(user.Language.CultureName);
                }

                HttpContext.Current.Session[SessionDataKey] = currentSessionData;
                HttpContext.Current.Session.Timeout = 43200;
            }
        }


        public static void RefreshSessionDataFor(User user)
        {
            var currentSessionData = new SessionData();

            if (user?.Language != null && !string.IsNullOrEmpty(user.Language.CultureName))
            {
                currentSessionData.m_userId = user.Id;
                SessionData.culture = user.Language.CultureName;

                Thread.CurrentThread.CurrentCulture = new CultureInfo(user.Language.CultureName);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(user.Language.CultureName);
            }
            HttpContext.Current.Session[SessionDataKey] = currentSessionData;
        }

        public static void ChangeCurrentLanguage(string culture)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                SessionData.culture = culture;

                Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
            }
        }

    }
}