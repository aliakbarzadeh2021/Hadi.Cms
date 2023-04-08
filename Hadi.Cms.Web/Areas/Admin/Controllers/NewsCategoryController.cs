using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Language.Resources;
using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Web.Utilities;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// دسته بندی
    /// </summary>
    public class NewsCategoryController : Controller
    {
        private readonly NewsCategoryService _newsCategoryService;
        private readonly NewsNewsCategoryService _newsNewsCategoryService;
        private readonly EventLoger _eventLoger;

        public NewsCategoryController()
        {
            _newsCategoryService = new NewsCategoryService();
            _newsNewsCategoryService= new NewsNewsCategoryService();
            _eventLoger = new EventLoger();
        }

        /// <summary>
        /// دریافت لیست دسته بندی ها
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int pageNumber = 1)
        {
            var pageSize = 5;
            var newsCategoryList = _newsCategoryService.GetList(null, o => o.OrderBy(c => c.OrderId));
            var pagedList = newsCategoryList.ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;

            return View(pagedList);
        }

        /// <summary>
        /// صفحه ی ثبت دسته بندی
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View(new NewsCategoryCreateCommand());
        }

        /// <summary>
        /// ثبت دسته بندی
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewsCategoryCreateCommand command)
        {
            try
            {
                _newsCategoryService.CreateNewCategory(command, SessionData.Current.User.Id);
                _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id,
                    SessionData.Current.User.UserName, "NewsCategoryController", "Create", "Success Create News Category",
                    HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// صفحه ی ویرایش دسته بندی
        /// </summary>
        /// <param name="newsCategoryId"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid newsCategoryId)
        {
            var newsCategory = _newsCategoryService.GetByID(newsCategoryId);
            if (newsCategory == null)
                return RedirectToAction("Index");

            return View(newsCategory);
        }

        /// <summary>
        /// ویرایش دسته بندی
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NewsCategoryEditCommand command)
        {
            try
            {
                var newsCategory = _newsCategoryService.GetByID(command.Id);
                if (newsCategory == null)
                    return RedirectToAction("Index");

                _newsCategoryService.UpdateNewsCategory(newsCategory.MapToEntity(), command, SessionData.Current.User.Id);
                _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id,
                    SessionData.Current.User.UserName, "NewsCategoryController", "Edit", "Success Update News Category",
                    HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// حذف یک دسته بندی
        /// </summary>
        /// <param name="newsCategoryId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(Guid newsCategoryId)
        {
            try
            {
                var newsNewsCategories = _newsNewsCategoryService.GetList(nc => nc.NewsCategoryId == newsCategoryId).MapToListEntity();
                foreach (var newsNewsCategory in newsNewsCategories)
                {
                    _newsNewsCategoryService.Delete(newsNewsCategory.Id);
                }

                _newsNewsCategoryService.Save();

                var newsCategory = _newsCategoryService.GetByID(newsCategoryId).MapToEntity();
                _newsCategoryService.DeleteById(newsCategory.Id);

                ViewBag.Message = Strings.NewsCategory_Delete_Success;
                ViewBag.Title = Strings.Global_Success;
                ViewBag.Type = "success";
                return Json(new
                {
                    Message = ViewBag.Message,
                    Title = ViewBag.Title,
                    Type = ViewBag.Type
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                ViewBag.Message = Strings.Global_SystemError;
                ViewBag.Title = Strings.Global_Error;
                ViewBag.Type = "error";

                return Json(new
                {
                    Message = ViewBag.Message,
                    Title = ViewBag.Title,
                    Type = ViewBag.Type
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}