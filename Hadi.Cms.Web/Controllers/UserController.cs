using Newtonsoft.Json;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Configuration;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Infrastructure.Utilities;
using Hadi.Cms.Infrastructure.Validations;
using Hadi.Cms.Language.Resources;
using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Model.QueryModels;
using Hadi.Cms.Notification.Email;
using Hadi.Cms.Notification.Sms;
using Hadi.Cms.Repository.UnitOfWork;
using Hadi.Cms.Web.Utilities;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;

namespace Hadi.Cms.Web.Controllers
{
    public class UserController : Controller
    {

        private DataContext _dataContext;
        private UserService _userService;
        private ConfirmKeyService _confirmKeyService;
        private UserProfileService _userProfileService;
        private LoginHistoryService _loginHistoryService;
        private EventLoger _eventLoger;
        private LanguageService _languageService;


        public UserController()
        {
            _dataContext = new DataContext();
            _userService = new UserService();
            _confirmKeyService = new ConfirmKeyService();
            _userProfileService = new UserProfileService();
            _loginHistoryService = new LoginHistoryService();
            _eventLoger = new EventLoger();
            _languageService = new LanguageService();
        }


        [HttpGet]
        public ActionResult Login(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View(new LoginViewModel());
        }


        [HttpPost]
        public ActionResult Login(LoginViewModel model, string ReturnUrl)
        {
            var loginViewModel = new LoginViewModel();

            if (Request.IsLocal && String.IsNullOrEmpty(model.Username))
            {
                User user = new User();
                user.Id = _userService.Get(x => x.UserName == "administrator").Id;
                user.UserName = "administrator";
                SessionData.CreateSessionDataFor(user);
                FormsAuthentication.RedirectFromLoginPage(user.UserName, true);

                if (string.IsNullOrEmpty(ReturnUrl))
                {
                    if (SessionData.Current.CurrentUserIsAdministrator)
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    else
                        return RedirectToAction("Index", "Home", new { area = "" });
                }
                else
                    return Redirect(ReturnUrl);
            }
            if (ModelState.IsValid)
            {
                User user;
                var loginResult = _userService.Authenticate(model.Username, model.Password, out user);

                // google recaptcha
                var recaptchaResponse = Request["g-recaptcha-response"];
                const string secret = "6LeecCITAAAAAHVQ0maUlzfeQ8MrDE8688uwPizZ";

                //var client = new WebClient();
                //var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
                //    secret, recaptchaResponse));
                //var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

                if ((Settings.Application_LoginWithCaptcha ) || (string)TempData["ShowCaptcha"] != "block") // check google recaptcha
                {
                    if (loginResult == UserAuthenticationResult.Successful)
                    {
                        // Login is Sucessful

                        SessionData.CreateSessionDataFor(user);
                        FormsAuthentication.RedirectFromLoginPage(user.UserName, false);
                    }
                    else if (loginResult == UserAuthenticationResult.InvalidUserNameOrPassword)
                    {
                        ViewBag.ErrorMessage = Strings.Views_Login_InvalidUsernameOrPassword;
                        var userWithUserName = _userService.Get(u => u.UserName.ToLower() == model.Username.ToLower()).MaptoEntity();
                        // set and check failed login limit
                        if (Settings.Application_FailedLoginLimitedCheck)
                        {
                            if (userWithUserName.LoginRetryCount >= Settings.Application_FailedLoginMaximumCount)
                            {
                                ViewBag.ErrorMessage = Strings.Views_Login_YourAccountHasBeenBlocked;
                                if (userWithUserName.IsEnable != false)
                                    userWithUserName.IsEnable = false;

                                _eventLoger.LogEvent(EventType.Warning, SessionData.Current.User.Id, SessionData.Current.User.UserName, "UserController", "Create", "Failed Login Limit ", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
                            }
                            else
                                userWithUserName.LoginRetryCount += 1;

                            _userService.Update(userWithUserName);
                            _userService.Save();
                        }

                        TempData["ShowCaptcha"] = Settings.Application_LoginWithCaptcha ? "block" : "none";
                        TempData.Keep("ShowCaptcha");
                        return View(loginViewModel);
                    }
                    else if (loginResult == UserAuthenticationResult.UserDisabledByAdmin)
                    {
                        ViewBag.ErrorMessage = Strings.Views_Login_YourAccountHasBeenBlocked;
                        TempData["ShowCaptcha"] = Settings.Application_LoginWithCaptcha ? "block" : "none";
                        TempData.Keep("ShowCaptcha");
                        return View(loginViewModel);
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = Strings.Views_Login_InvalidUsernameOrPassword;

                    // set and check failed login limit
                    if (Settings.Application_FailedLoginLimitedCheck)
                    {
                        var userWithUserName = _userService.Get(u => u.UserName.ToLower() == model.Username.ToLower()).MaptoEntity();

                        if (userWithUserName.LoginRetryCount >= Settings.Application_FailedLoginMaximumCount)
                        {
                            ViewBag.ErrorMessage = Strings.Views_Login_YourAccountHasBeenBlocked;
                            if (userWithUserName.IsEnable != false)
                                userWithUserName.IsEnable = false;

                            _eventLoger.LogEvent(EventType.Warning, SessionData.Current.User.Id, SessionData.Current.User.UserName, "UserController", "Create", "Failed Login Limit ", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
                        }
                        else
                            userWithUserName.LoginRetryCount += 1;

                        _userService.Update(userWithUserName);
                        _userService.Save();
                    }

                    TempData["ShowCaptcha"] = Settings.Application_LoginWithCaptcha ? "block" : "none";
                    TempData.Keep("ShowCaptcha");
                    return View(loginViewModel);
                }

                if (string.IsNullOrEmpty(ReturnUrl))
                {
                    if (SessionData.Current.CurrentUserIsAdministrator)
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }
                }
                else
                {
                    return Redirect(ReturnUrl);
                }
            }
            else
            {
                ViewBag.ErrorMessage = Strings.Views_Login_InvalidUsernameOrPassword;

                // set and check failed login limit
                if (Settings.Application_FailedLoginLimitedCheck)
                {
                    var userWithUserName = _userService.Get(u => u.UserName.ToLower() == model.Username.ToLower());

                    if (userWithUserName.LoginRetryCount >= Settings.Application_FailedLoginMaximumCount)
                    {
                        ViewBag.ErrorMessage = Strings.Views_Login_YourAccountHasBeenBlocked;
                        if (userWithUserName.IsEnable != false)
                            userWithUserName.IsEnable = false;
                    }
                    else
                        userWithUserName.LoginRetryCount += 1;
                    _userService.Save();
                }

                TempData["ShowCaptcha"] = Settings.Application_LoginWithCaptcha ? "block" : "none";
                TempData.Keep("ShowCaptcha");
                return View(loginViewModel);
            }
        }


        [HttpGet]
        public ActionResult OtherLogin(bool? ForgetPassword)
        {
            if (ForgetPassword == true)
            {
                ViewBag.ForgetPasswordText = "درصورتی که کلمه عبور خود را فراموش کرده اید، می توانید با وارد کردن اطلاعات ثبت شده خود، مجددا کد فعال سازی حساب خود را دریافت کنید.";
                return View();
            }

            if (Settings.Application_LoginWithEmail || Settings.Application_LoginWithSms)
            {
                if (Settings.Application_LoginWithEmail && Settings.Application_LoginWithSms)
                {
                    ViewBag.EnterText = Strings.Global_EmailOrSms;
                }
                else if (Settings.Application_LoginWithEmail && !Settings.Application_LoginWithSms)
                {
                    ViewBag.EnterText = Strings.Global_Email;
                }
                else if (!Settings.Application_LoginWithEmail && Settings.Application_LoginWithSms)
                {
                    ViewBag.EnterText = Strings.Global_Sms;
                }

                ViewBag.EnterText += " " + Strings.ToReceiveActivationCode;

                return View();
            }
            else
            {
                return RedirectToAction("Login", "User", new { area = "" });
            }
        }

        [HttpPost]
        public ActionResult OtherLogin(string loginField, string EnterText)
        {
            ViewBag.EnterText = EnterText;

            if (!String.IsNullOrEmpty(loginField))
            {
                // google recaptcha
                var recaptchaResponse = Request["g-recaptcha-response"];
                const string secret = "6LeecCITAAAAAHVQ0maUlzfeQ8MrDE8688uwPizZ";

                var client = new WebClient();
                var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
                    secret, recaptchaResponse));
                var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

                if ((Settings.Application_LoginWithCaptcha && captchaResponse.Success) || (string)TempData["ShowCaptcha"] != "block") // check google recaptcha
                {
                    if (loginField.Contains("@") && ValidationHelper.IsValidEmail(loginField))
                    {
                        var newKey = new ConfirmKey()
                        {
                            IsEmail = true,
                            IsSms = false,
                            CreateDate = DateTime.Now,
                            ExpireDate = DateTime.Now.AddHours(72),
                            LinkGuid = Guid.NewGuid(),
                            SmsKey = null,
                            UserEmailAddress = loginField,
                            UserMobileNumber = null
                        };
                        _confirmKeyService.Insert(newKey);
                        _confirmKeyService.Save();

                        // send mail to user
                        string link = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority + "/User/ConfirmKey?keyCode=" + newKey.LinkGuid;
                        string message = @"<p>کاربر گرامی</p>" +
                            "<p>لینک زیر طبق درخواست شما و جهت تغییر رمز عبور و فعال سازی حساب کاربری برای شما ارسال شده است:</p>" +
                            "<a href='" + link + "'>" + link + "</a><p></p>" +
                            "<p>درصورتی که این درخواست از طرف شما در سیستم ایجاد نشده، لطفا آنرا نادیده بگیرید. این درخواست ظرف مدت 72 ساعت منقضی خواهد شد.</p>";

                        var ep = new EmailProvider();
                        ep.SendMail(loginField, "درخواست تغییر رمز عبور و فعال سازی حساب کاربری", message);

                        ViewBag.Message = "تا 15 دقیقه آینده، یک پیام حاوی لینک فعال سازی، به آدرس پست الکترونیک شما ارسال خواهد شد.";
                        TempData["ShowCaptcha"] = Settings.Application_LoginWithCaptcha ? "block" : "none";
                        TempData.Keep("ShowCaptcha");

                        return View();
                    }
                    else if (loginField.StartsWith("+989") || loginField.StartsWith("09"))
                    {
                        var newKey = new ConfirmKey()
                        {
                            IsEmail = false,
                            IsSms = true,
                            CreateDate = DateTime.Now,
                            ExpireDate = DateTime.Now.AddHours(72),
                            LinkGuid = null,
                            SmsKey = Utility.GenerateUniqueOrderId().ToString().Substring(0, 6),
                            UserEmailAddress = null,
                            UserMobileNumber = loginField
                        };
                        _confirmKeyService.Insert(newKey);
                        _confirmKeyService.Save();

                        string message = @"کد فعال سازی حساب کاربری شما : " + newKey.SmsKey;
                        message += Environment.NewLine;
                        message += "شرکت هادی | سامانه مدیریت محتوا";

                        var sms = new SmsIr();
                        sms.SendMessageRestful(loginField.Replace("+989", "09"), message);

                        ViewBag.Message = @"تا 10 دقیقه آینده، یک پیام حاوی کد فعال سازی، به شماره تلفن همراه شما ارسال خواهد شد. لطفا پس از دریافت آن، کد فعال سازی را در صفحه مربوط به لینک زیر وارد کنید." +
                            "<p></p><a href='/User/ConfirmKey'>بررسی کد فعالسازی</a><p></p>";
                        TempData["ShowCaptcha"] = Settings.Application_LoginWithCaptcha ? "block" : "none";
                        TempData.Keep("ShowCaptcha");

                        //just for myopt:
                        return RedirectToAction("ConfirmKey");
                    }
                    else
                    {
                        //ViewBag.ErrorMessage = Strings.Views_Login_InvalidFormat;
                        TempData["ShowCaptcha"] = Settings.Application_LoginWithCaptcha ? "block" : "none";
                        TempData.Keep("ShowCaptcha");

                        return View();
                    }
                }
                else
                {
                    //ViewBag.ErrorMessage = Strings.Views_Login_InvalidCaptchaCode;
                    TempData["ShowCaptcha"] = Settings.Application_LoginWithCaptcha ? "block" : "none";
                    TempData.Keep("ShowCaptcha");

                    return View();
                }
            }
            else
            {
                //ViewBag.ErrorMessage = Strings.Views_Login_InvalidFormat;
                TempData["ShowCaptcha"] = Settings.Application_LoginWithCaptcha ? "block" : "none";
                TempData.Keep("ShowCaptcha");

                return View();
            }
        }

        public ActionResult ConfirmKey(string keyCode)
        {
            Guid resultGuid = new Guid();
            if (string.IsNullOrEmpty(keyCode))
            {
                return View();
            }
            else if (Guid.TryParse(keyCode, out resultGuid))
            {
                User user;
                var item = _confirmKeyService.GetList().OrderByDescending(o => o.CreateDate).FirstOrDefault(c => c.LinkGuid == resultGuid && c.ExpireDate > DateTime.Now);
                if (item != null)
                {
                    //var test  = _userService.Get(u => u.UserName == item.UserEmailAddress);

                    user = _userService.Get(u => u.UserName == item.UserEmailAddress).MaptoEntity();
                    if (user == null)
                    {
                        var profile = _userProfileService.Get(u => u.EmailAddress == item.UserEmailAddress);
                        if (profile != null)
                        {
                            //user = profile.UserId;
                        }
                    }

                    if (user != null) // آدرس ایمیل در سیستم وجود دارد
                    {
                        if (user.IsEnable && !user.IsDeleted)
                        {
                            User currentUser = new User();
                            currentUser.Id = user.Id;
                            currentUser.UserName = user.UserName;
                            SessionData.CreateSessionDataFor(currentUser);
                            FormsAuthentication.RedirectFromLoginPage(currentUser.UserName, true);

                            return RedirectToAction("ChangePassword", "Users", new { area = "Admin", userId = currentUser.Id });
                        }
                        else
                        {
                            ViewBag.Message = "کاربر مورد نظر در سیستم در وضعیت غیرفعال است. لطفا با مدیر سایت تماس بگیرید.";
                            return View();
                        }
                    }
                    else
                    {
                        var result = _userService.CreateNewUser(Strings.Global_User, Strings.Global_CreateNew, true, item.UserEmailAddress, Guid.NewGuid().ToString().Substring(0, 7), true);
                        if (result)
                        {
                            User currentUser = new User();
                            currentUser.Id = _userService.Get(x => x.UserName == item.UserEmailAddress).Id;
                            currentUser.UserName = item.UserEmailAddress;
                            SessionData.CreateSessionDataFor(currentUser);
                            FormsAuthentication.RedirectFromLoginPage(currentUser.UserName, true);

                            return RedirectToAction("ChangePassword", "Users", new { area = "Admin", userId = currentUser.Id });
                        }
                        else
                        {
                            ViewBag.Message = "خطا در ثبت کاربر!";
                            return View();
                        }
                    }
                }
            }
            ViewBag.Message = "هیچ موردی یافت نشد";
            return View();
        }

        public ActionResult ConfirmSmsKey(string keyCode)
        {
            if (!string.IsNullOrEmpty(keyCode))
            {
                User user;
                var item = _confirmKeyService.GetList().OrderByDescending(o => o.CreateDate).FirstOrDefault(c => c.SmsKey == keyCode && c.ExpireDate > DateTime.Now);
                if (item != null)
                {
                    user = _userService.Get(u => u.UserName == item.UserMobileNumber).MaptoEntity();
                    if (user == null)
                    {
                        var profile = _userProfileService.Get(u => u.MobileNumber == item.UserMobileNumber);
                        if (profile != null)
                        {
                            //user = profile.User;
                        }
                    }

                    if (user != null) // آدرس ایمیل در سیستم وجود دارد
                    {
                        if (user.IsEnable && !user.IsDeleted)
                        {
                            User currentUser = new User();
                            currentUser.Id = user.Id;
                            currentUser.UserName = user.UserName;
                            SessionData.CreateSessionDataFor(currentUser);
                            FormsAuthentication.RedirectFromLoginPage(currentUser.UserName, true);

                            return RedirectToAction("ChangePassword", "Users", new { area = "Admin", userId = currentUser.Id });
                        }
                        else
                        {
                            ViewBag.Message = "کاربر مورد نظر در سیستم در وضعیت غیرفعال است. لطفا با مدیر سایت تماس بگیرید.";
                            return View();
                        }
                    }
                    else
                    {
                        var result = _userService.CreateNewUser(Strings.Global_User, Strings.Global_CreateNew, true, item.UserMobileNumber, Guid.NewGuid().ToString().Substring(0, 7), true);
                        if (result)
                        {
                            User currentUser = new User();
                            currentUser.Id = _userService.Get(x => x.UserName == item.UserMobileNumber).Id;
                            currentUser.UserName = item.UserMobileNumber;
                            SessionData.CreateSessionDataFor(currentUser);
                            FormsAuthentication.RedirectFromLoginPage(currentUser.UserName, true);

                            return RedirectToAction("ChangePassword", "Users", new { area = "Admin", userId = currentUser.Id });
                        }
                        else
                        {
                            ViewBag.Message = "خطا در ثبت کاربر!";
                            return View();
                        }
                    }
                }
            }
            ViewBag.Message = "هیچ موردی یافت نشد";
            return View("ConfirmKey");
        }


        public ActionResult LogOff()
        {
            if (User.Identity.IsAuthenticated)
            {
                // log in loginHistory table
                var newLog = new LoginHistory()
                {
                    IsLogin = false,
                    CreateDate = DateTime.Now,
                    UserId = SessionData.Current.User.Id
                };

                _loginHistoryService.Insert(newLog);
                _loginHistoryService.Save();

                Session.Abandon();
                FormsAuthentication.SignOut();



                string culture = Utility.GetCultureFromCookie().Name;
                SessionData.ChangeCurrentLanguage(culture);
            }

            return Redirect(FormsAuthentication.DefaultUrl);
        }

        public ActionResult SetCulture(string culture, string currentUrl)
        {
            if (SessionData.Current.User != null)
            {
                var user = _userService.Get(u => u.Id == SessionData.Current.User.Id).MaptoEntity();
                string cultureCode = new System.Globalization.CultureInfo(culture).LCID.ToString();
                var language = _languageService.Get(q => q.CultureCode == cultureCode);
                user.LanguageId = language.Id;
                _userService.Update(user);
                _userService.Save();
            }
            SessionData.ChangeCurrentLanguage(culture);

            return Redirect(currentUrl);
        }

    }
}