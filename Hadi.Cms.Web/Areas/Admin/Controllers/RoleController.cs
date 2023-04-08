using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.QueryModels;
using Hadi.Cms.Web.Controllers;
using Hadi.Cms.Web.Utilities;
using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    public class RoleController : BaseController
    {
        private RoleService _roleService;
        private UserRoleService _userRoleService;
        private UserService _userService;
        private EventLoger _eventLoger;

        public RoleController()
        {
            _roleService = new RoleService();
            _userRoleService = new UserRoleService();
            _userService = new UserService();
            _eventLoger = new EventLoger();
        }

        public ActionResult List(int pageNumber = 1)
        {
            var pageSize = 5;
            var roles = _roleService.GetList(null, o => o.OrderByDescending(r => r.Name));
            var pagedList = roles.ToPagedList(pageNumber, pageSize);
            ViewBag.PageNumber = pageNumber;
            return View(pagedList);
        }

        public ActionResult Create()
        {
            return View(new RoleModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoleModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var item = _roleService.Get(u => u.Id == model.Id);
                if (item == null)
                {
                    var newRole = new Role()
                    {
                        IsActive = true,
                        DisplayName = model.DisplayName,
                        Name = model.Name
                    };

                    _roleService.Insert(newRole);
                    _roleService.Save();

                    _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "RolesController", "Create", "Success Create Role", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);

                    ViewBag.RoleId = newRole.Id;
                    ViewBag.Message = Strings.Role_Successfully_Created;
                    ViewBag.Success = "1";
                }
                else
                {
                    ViewBag.Message = Strings.Role_Exists_System;
                    ViewBag.Success = "0";
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteRole(Guid roleId)
        {
            try
            {
                var item = _roleService.Get(q => q.Id == roleId);

                if (item != null /*&& item.ItemID > 4*/) // چهارتای اول سیستمی هستند
                {
                    var assignedRoleToUser = _userRoleService.Any(ur => ur.RoleId == roleId);
                    if (assignedRoleToUser)
                    {
                        ViewBag.Message = Strings.Role_Assigned_To_User_Error;
                        ViewBag.Success = Strings.Error;
                        return Json(new
                        {
                            Message = ViewBag.Message,
                            Success = ViewBag.Success
                        }, JsonRequestBehavior.AllowGet);
                    }

                    _roleService.DeleteById(item.Id);
                    _roleService.Save();

                    ViewBag.Message = Strings.Role_Deleted_Successfully;
                    ViewBag.Success = Strings.Success;
                }
                else
                {
                    ViewBag.Message = Strings.Role_Delete_Dont_Have_Permission;
                    ViewBag.Success = Strings.Error;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = Strings.Role_Error_While_Delete;
                ViewBag.Success = Strings.Error;
            }

            return Json(new
            {
                Message = ViewBag.Message,
                Success = ViewBag.Success
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RoleUsers(Guid roleId)
        {
            var role = _roleService.Get(q => q.Id == roleId);
            var users = _userService.GetList();
            ViewBag.RoleId = roleId;
            ViewBag.RoleName = role.Name;
            ViewBag.Users = users;
            return View();
        }

        [HttpPost]
        public ActionResult SetUserToRole(string UserId, Guid RoleId)
        {
            var userGuid = Guid.Parse(UserId);
            string Type = "";
            var item = _userRoleService.Get(r => r.RoleId == RoleId && r.UserId == userGuid);
            if (item == null)
            {
                var newItem = new UserRole()
                {
                    UserId = userGuid,
                    RoleId = RoleId
                };
                _userRoleService.Insert(newItem);

                _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "RolesController", "Create", "Success Create UserRole", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
                
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
                userId = userGuid
            }, JsonRequestBehavior.AllowGet);
        }
    }
}