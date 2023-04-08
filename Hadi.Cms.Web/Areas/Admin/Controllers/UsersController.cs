using Hadi.Cms.ApplicationService.QueryModels;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Configuration;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Model.QueryModels;
using Hadi.Cms.Web.Controllers;
using Hadi.Cms.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    public class UsersController : BaseController
    {
        private UserService _userService;
        private LanguageService _languageService;
        private LoginHistoryService _loginHistoryService;
        private UserRoleService _userRoleService;
        private RoleService _roleService;
        private EventLoger _eventLoger;

        public UsersController()
        {
            _userService = new UserService();
            _loginHistoryService = new LoginHistoryService();
            _userRoleService = new UserRoleService();
            _userRoleService = new UserRoleService();
            _languageService = new LanguageService();
            _roleService = new RoleService();
            _eventLoger = new EventLoger();
        }

        public ActionResult Index()
        {
            var model = new UserDto();
            model.UserListCollctionDto = new List<IUserDto>();

            if (SessionData.Current.CurrentUserIsAdministrator)
                model.UserListCollctionDto.AddRange(_userService.GetList(q => !q.IsDeleted));
            else
                model.UserListCollctionDto.AddRange(_userService.GetList(q => !q.IsDeleted && q.UserName.ToLower() != "administrator"));

            // user count
            var systemUserCount = _userService.Count(l => !l.IsDeleted);
            ViewBag.AllUserCount = systemUserCount;

            var yesterday = DateTime.Now.AddHours(-24);
            var onlineUserCount = _loginHistoryService.Where(l => l.IsLogin && l.CreateDate > yesterday && !l.User.IsDeleted).OrderByDescending(o => o.CreateDate).Select(l => l.UserId).Distinct().ToList().Count();
            ViewBag.OnlineUserCount = onlineUserCount;

            var disableUserCount = _userService.Count(u => !u.IsDeleted && !u.IsEnable);
            ViewBag.DisableUserCount = disableUserCount;

            ViewBag.OfflineUserCount = ((systemUserCount - onlineUserCount) - disableUserCount);
            // ----------

            return View(model);
        }

        public ActionResult Create()
        {
            ViewBag.Languages = _languageService.GetList();
            return View(new UserModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserModel model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.UserName) && !string.IsNullOrEmpty(model.Password))
                {
                    var item = _userService.Get(u => u.UserName.ToLower() == model.UserName.ToLower());
                    if (item == null)
                    {
                        var newUser = new User()
                        {
                            Id = Guid.NewGuid(),
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            BuiltIn = false,
                            IsDeleted = false,
                            MostChangePassword = true,
                            LoginRetryCount = 0,
                            IsEnable = model.IsEnabled,
                            UserName = model.UserName,
                            LanguageId = model.LanguageId
                        };

                        newUser.SetPassword(model.Password);

                        _userService.Insert(newUser);
                        _userService.Save();

                        _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "UsersController", "Create", "Success Create User", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);

                        ViewBag.UserId = newUser.Id;
                        ViewBag.Message = Strings.Success_User_Signup;
                        ViewBag.Success = "1";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Message = Strings.User_Name_Already_Taken;
                        ViewBag.Success = "0";
                    }
                }
                else
                {
                    ViewBag.Message = Strings.Create_Failed;
                    ViewBag.Success = "0";
                }
            }
            ViewBag.Languages = _languageService.GetList();
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteUser(Guid UserId)
        {
            try
            {
                var item = _userService.Get(q => q.Id == UserId).MaptoEntity();
                if (item != null && item.UserName.ToLower() != "administrator" && item.UserName.ToLower() != "publicuser")
                {
                    item.IsDeleted = true;
                    item.IsEnable = false;
                    _userService.Update(item);
                    _userService.Save();

                    ViewBag.Message = Strings.User_Successfully_Deleted;
                    ViewBag.Success = Strings.Success;
                }
                else
                {
                    ViewBag.Message = Strings.User_NotPermission_To_Delete_Error;
                    ViewBag.Success = Strings.Error;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = Strings.User_Failed_Deleted;
                ViewBag.Success = Strings.Error;
            }

            return Json(new
            {
                Message = ViewBag.Message,
                Success = ViewBag.Success
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeStatus(Guid userId)
        {
            var item = _userService.Get(q => q.Id == userId).MaptoEntity();
            if (item != null)
            {
                item.IsEnable = !item.IsEnable;

                if (Settings.Application_FailedLoginLimitedCheck && item.IsEnable)
                    item.LoginRetryCount = 0;

                _userService.Update(item);
                _userService.Save();
            }
            return RedirectToAction("Index");
        }

        public ActionResult ChangePassword(Guid userId)
        {
            ViewBag.UserId = userId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string password, Guid userId)
        {
            if (userId != null && password != null)
            {
                var item = _userService.Get(q => q.Id == userId).MaptoEntity();
                if (item != null)
                {
                    item.SetPassword(password);
                    if (item.UserName.ToLower() != "publicuser" && item.Id != SessionData.Current.User.Id)
                    {
                        item.MostChangePassword = true;
                    }
                    else
                    {
                        item.MostChangePassword = false;
                    }
                    _userService.Update(item);
                    _userService.Save();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = Strings.Change_Password_Error;
                    ViewBag.Success = "0";
                }
            }
            ViewBag.UserId = userId;
            return View();
        }

        public ActionResult UserRoles(Guid userId)
        {
            ViewBag.UserId = userId;
            var user = _userService.Get(q => q.Id == userId);
            ViewBag.UserFullName = user.FullName;

            var roles = _roleService.GetList();
            ViewBag.Roles = roles;

            return View(user);
        }

        [HttpPost]
        public ActionResult SetRoleToUser(Guid RoleId, Guid UserId)
        {
            string Type = "";
            var item = _userRoleService.Get(r => r.RoleId == RoleId && r.UserId == UserId);
            if (item == null)
            {
                var newItem = new UserRole()
                {
                    UserId = UserId,
                    RoleId = RoleId
                };
                _userRoleService.Insert(newItem);
                Type = "add";
            }
            else
            {
                _userRoleService.DeleteById(item.Id);
                Type = "remove";
            }
            _userRoleService.Save();

            return Json(new
            {
                type = Type,
                roleId = RoleId
            }, JsonRequestBehavior.AllowGet);
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

                SessionData.ChangeCurrentLanguage(culture);
            }
            return Redirect(currentUrl);
        }

    }
}