using PagedList;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Infrastructure.Helpers;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Model.QueryModels;
using Hadi.Cms.Web.Controllers;
using Hadi.Cms.Web.Utilities;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    public class MenusController : BaseController
    {
        private MenuService _menuService;
        private AttachmentFileService _attachmentFileService;
        private LanguageService _languageService;
        private EventLoger _eventLoger;

        public MenusController()
        {
            _menuService = new MenuService();
            _attachmentFileService = new AttachmentFileService();
            _languageService = new LanguageService();
            _eventLoger = new EventLoger();
        }

        public ActionResult Index(int pageNumber = 1)
        {
            int pageSize = 5;

            var menus = _menuService.GetList(q => q.IsDeleted == false && q.CreatedBy == SessionData.Current.User.Id);

            foreach (var menu in menus)
                menu.ParentTitle = _menuService.Get(m => m.Id == menu.ParentId)?.Title;

            var pagedListMenus = menus.OrderBy(o => o.OrderId).ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;
            TempData["PageNumber"] = pageNumber;
            TempData.Keep("PageNumber");

            return View(pagedListMenus);
        }

        public ActionResult Create()
        {
            string[] rels = { "alternate", "author", "bookmark", "help", "license", "next", "nofollow", "noreferrer", "prefetch", "prev", "search" };
            ViewBag.Rels = rels;

            string[] targets = { "_blank", "_self", "_parent", "_top" };
            ViewBag.Targets = targets;

            ViewBag.Menus = _menuService.GetList(m => m.IsActive && m.IsParent && !m.IsDeleted && m.CreatedBy == SessionData.Current.User.Id);

            ViewBag.Languages = _languageService.GetList();

            return View(new MenuModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MenuModel model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    byte[] imageData = null;
                    var fileSize = file.ContentLength;
                    using (var binaryReader = new System.IO.BinaryReader(file.InputStream))
                        imageData = binaryReader.ReadBytes(file.ContentLength);

                    var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(file.FileName));
                    var imageWidth = 0;
                    var imageHeight = 0;
                    if (mimeType.Contains("image"))
                    {
                        var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                        imageWidth = img.Width;
                        imageHeight = img.Height;
                    }

                    var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(file.FileName),
                         System.IO.Path.GetExtension(file.FileName), fileSize, mimeType,
                         imageWidth, imageHeight, "MenuImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                    model.ImageId = attachmentId;
                }

                var userId = SessionData.Current.User.Id;

                var menu = new Menu
                {
                    CreatedBy = userId,
                    LanguageId = model.LanguageId,
                    Link = !string.IsNullOrEmpty(model.Link) ? model.Link : "#",
                    OrderId = model.OrderId,
                    ParentId = model.ParentId,
                    Relationship = model.Relationship,
                    Target = model.Target,
                    Title = model.Title,
                    ImageId = model.ImageId,
                    CssClass = model.CssClass,
                    IsActive = model.IsActive,
                    IsSideBar = model.IsSideBar,
                    Color = model.Color,
                    ModifiedBy = userId,
                    ModifiedDate = DateTime.Now,
                    IsParent = model.IsParent
                };

                _menuService.Insert(menu);
                _menuService.Save();

                _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "MenusController", "Create", "Success Create Menu", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
            }

            ViewBag.Menus = _menuService.GetList(m => m.IsActive && m.IsParent && !m.IsDeleted && m.CreatedBy == SessionData.Current.User.Id);

            ViewBag.Languages = _languageService.GetList();
            string[] rels = { "alternate", "author", "bookmark", "help", "license", "next", "nofollow", "noreferrer", "prefetch", "prev", "search" };
            ViewBag.Rels = rels;

            string[] targets = { "_blank", "_self", "_parent", "_top" };
            ViewBag.Targets = targets;
            return View(model);
        }



        public ActionResult Edit(Guid Id)
        {
            var menu = _menuService.GetById(Id);
            if (menu == null)
                return RedirectToAction("Index", new { pageNumber = int.Parse(TempData["PageNumber"].ToString()) });

            ViewBag.Menus = _menuService.GetList(m => m.IsActive && m.IsParent && m.Id != menu.Id && !m.IsDeleted && m.CreatedBy == SessionData.Current.User.Id);

            ViewBag.Languages = _languageService.GetList();

            var menuModel = new MenuModel
            {
                CreateDate = menu.CreateDate,
                CreatedBy = menu.CreatedBy,
                CssClass = menu.CssClass,
                Id = menu.Id,
                ImageId = menu.ImageId,
                LanguageId = menu.LanguageId,
                Link = menu.Link,
                OrderId = menu.OrderId,
                ParentId = menu.ParentId,
                Relationship = menu.Relationship,
                Target = menu.Target,
                Title = menu.Title,
                IsActive = menu.IsActive,
                ModifiedBy = menu.ModifiedBy,
                ModifiedDate = menu.ModifiedDate,
                IsSideBar = menu.IsSideBar,
                Color = menu.Color,
                IsParent = menu.IsParent
            };

            string[] rels = { "alternate", "author", "bookmark", "help", "license", "next", "nofollow", "noreferrer", "prefetch", "prev", "search" };
            ViewBag.Rels = rels;

            string[] targets = { "_blank", "_self", "_parent", "_top" };
            ViewBag.Targets = targets;

            return View(menuModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MenuModel model, HttpPostedFileBase file)
        {
            var menu = _menuService.GetById(model.Id).MaptoEntity();

            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    if (menu.ImageId.HasValue)
                        _attachmentFileService.RemoveAttachment(menu.ImageId.Value);

                    byte[] imageData = null;
                    var fileSize = file.ContentLength;
                    using (var binaryReader = new System.IO.BinaryReader(file.InputStream))
                        imageData = binaryReader.ReadBytes(file.ContentLength);

                    var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(file.FileName));
                    var imageWidth = 0;
                    var imageHeight = 0;
                    if (mimeType.Contains("image"))
                    {
                        var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                        imageWidth = img.Width;
                        imageHeight = img.Height;
                    }

                    var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(file.FileName),
                         System.IO.Path.GetExtension(file.FileName), fileSize, mimeType,
                         imageWidth, imageHeight, "MenuImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                    model.ImageId = attachmentId;
                }

                var userId = SessionData.Current.User.Id;

                menu.LanguageId = model.LanguageId;
                menu.Link = !string.IsNullOrEmpty(model.Link) ? model.Link : "#";
                menu.OrderId = model.OrderId;
                menu.ParentId = model.ParentId;
                menu.Relationship = model.Relationship;
                menu.Target = model.Target;
                menu.Title = model.Title;
                menu.ImageId = model.ImageId;
                menu.CssClass = model.CssClass;
                menu.IsActive = model.IsActive;
                menu.ModifiedBy = userId;
                menu.ModifiedDate = DateTime.Now;
                menu.IsSideBar = model.IsSideBar;
                menu.IsSideBar = model.IsSideBar;
                menu.Color = model.Color;
                menu.ModifiedBy = userId;
                menu.ModifiedDate = DateTime.Now;
                menu.IsParent = model.IsParent;
                _menuService.Update(menu);
                _menuService.Save();

                return RedirectToAction("Index", new { pageNumber = int.Parse(TempData["PageNumber"].ToString()) });
            }

            ViewBag.Menus = _menuService.GetList(m => m.IsActive && m.IsParent && m.Id != menu.Id && !m.IsDeleted && m.CreatedBy == SessionData.Current.User.Id);

            ViewBag.Languages = _languageService.GetList();
            string[] rels = { "alternate", "author", "bookmark", "help", "license", "next", "nofollow", "noreferrer", "prefetch", "prev", "search" };
            ViewBag.Rels = rels;

            string[] targets = { "_blank", "_self", "_parent", "_top" };
            ViewBag.Targets = targets;
            return View(model);
        }


        [HttpPost]
        public ActionResult Delete(Guid Id)
        {
            try
            {
                var menu = _menuService.GetById(Id);

                if (menu != null)
                {
                    var menuEntity = menu.MaptoEntity();
                    menuEntity.IsDeleted = true;
                    menuEntity.ModifiedDate = DateTime.Now;
                    menuEntity.ModifiedBy = SessionData.Current.User.Id;
                    _menuService.Update(menuEntity);
                    _menuService.Save();

                    ViewBag.Message = @Strings.Menu_Delete_Successfully;
                    ViewBag.Success = @Strings.Success;
                }
                else
                {
                    ViewBag.Message = Strings.Menu_Delete_Dont_Have_Permission;
                    ViewBag.Success = @Strings.Error;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = Strings.Menu_Delete_Error;
                ViewBag.Success = Strings.Error;
            }

            return Json(new
            {
                Message = ViewBag.Message,
                Success = ViewBag.Success
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeStatus(Guid menuId)
        {
            var menu = _menuService.GetById(menuId).MaptoEntity();
            if (menu != null)
            {
                menu.IsActive = !menu.IsActive;
                _menuService.Update(menu);
                _menuService.Save();
            }

            return RedirectToAction("Index", new { pageNumber = int.Parse(TempData["PageNumber"].ToString()) });
        }
    }
}