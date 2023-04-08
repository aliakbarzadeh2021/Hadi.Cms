using System;
using System.Collections.Generic;
using System.Linq;
using Hadi.Cms.ApplicationService.Services;
using System.Web.Mvc;
using PagedList;
using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.ApplicationService.QueryModels;
using Hadi.Cms.Infrastructure.Helpers;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Language.Resources;
using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Model.Enums;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Web.Utilities;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// مدیریت دسته بندی های فوتر
    /// </summary>
    public class FooterCategoriesController : Controller
    {
        private readonly FooterCategoryService _footerCategoryService;
        private readonly FooterCategoryLinkService _footerCategoryLinkService;
        private readonly EventLoger _eventLoger;


        public FooterCategoriesController()
        {
            _footerCategoryService = new FooterCategoryService();
            _footerCategoryLinkService = new FooterCategoryLinkService();
            _eventLoger = new EventLoger();
        }

        /// <summary>
        /// دریافت لیست دسته بندی ها
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public ActionResult Index(int pageNumber = 1)
        {
            var pageSize = 5;
            var footerCategories = _footerCategoryService.GetList(
                f => !f.IsDeleted && f.CreatedBy == SessionData.Current.User.Id,
                o => o.OrderBy(f => f.OrderId));
            var pagedList = footerCategories.ToPagedList(pageNumber, pageSize);
            ViewBag.PageNumber = pageNumber;
            
            TempData["PageNumber"] = pageNumber;
            TempData.Keep("PageNumber");

            return View(pagedList);
        }

        /// <summary>
        /// صفحه ی ثبت دسته بندی
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var entityHaveReviewCountList =
                Enum.GetValues(typeof(EntityHaveReviewCount)).OfType<EntityHaveReviewCount>().ToList();

            var entityHaveReviewCountDisplayAttrList = new List<EntityHaveReviewCountDto>();

            foreach (var item in entityHaveReviewCountList)
            {
                var entityHaveReviewCount = EnumHelper.GetDisplayForEnumValue(item);

                entityHaveReviewCountDisplayAttrList.Add(new EntityHaveReviewCountDto()
                {
                    Name = entityHaveReviewCount,
                    Value = (int)item
                });
            }

            ViewBag.EntityHaveReviewCountList = entityHaveReviewCountDisplayAttrList;

            return View(new FooterCategoryCreateCommand());
        }

        /// <summary>
        /// ثبت دسته بندی
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FooterCategoryCreateCommand command)
        {
            if (!ModelState.IsValid)
            {
                var entityHaveReviewCountList =
                    Enum.GetValues(typeof(EntityHaveReviewCount)).OfType<EntityHaveReviewCount>().ToList();

                var entityHaveReviewCountDisplayAttrList = new List<EntityHaveReviewCountDto>();

                foreach (var item in entityHaveReviewCountList)
                {
                    var entityHaveReviewCount = EnumHelper.GetDisplayForEnumValue(item);

                    entityHaveReviewCountDisplayAttrList.Add(new EntityHaveReviewCountDto()
                    {
                        Name = entityHaveReviewCount,
                        Value = (int)item
                    });
                }

                ViewBag.EntityHaveReviewCountList = entityHaveReviewCountDisplayAttrList;

                return View(command);
            }

            _footerCategoryService.AddFooterCategory(command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "FooterCategoriesController", "Create", "Success Create Footer Category", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);

            return RedirectToAction("Index" , new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1});
        }

        /// <summary>
        /// صفحه ی ویرایش دسته بندی
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {
            var footerCategory = _footerCategoryService.Get(id);
            if (footerCategory == null)
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            var entityHaveReviewCountList =
                Enum.GetValues(typeof(EntityHaveReviewCount)).OfType<EntityHaveReviewCount>().ToList();

            var entityHaveReviewCountDisplayAttrList = new List<EntityHaveReviewCountDto>();

            foreach (var item in entityHaveReviewCountList)
            {
                var entityHaveReviewCount = EnumHelper.GetDisplayForEnumValue(item);

                entityHaveReviewCountDisplayAttrList.Add(new EntityHaveReviewCountDto()
                {
                    Name = entityHaveReviewCount,
                    Value = (int)item
                });
            }

            ViewBag.EntityHaveReviewCountList = entityHaveReviewCountDisplayAttrList;

            return View(new FooterCategoryEditCommand
            {
                Id = footerCategory.Id,
                Title = footerCategory.Title,
                Link = footerCategory.Link,
                NumberOfShows = footerCategory.NumberOfShows,
                EntityHaveReviewCount = footerCategory.EntityHaveReviewCount,
                StatisticalRepresentation = footerCategory.StatisticalRepresentation,
                OrderId = footerCategory.OrderId,
                IsActive = footerCategory.IsActive
            });
        }

        /// <summary>
        /// ویرایش دسته بندی فوتر
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FooterCategoryEditCommand command)
        {
            if (!ModelState.IsValid)
            {
                var entityHaveReviewCountList =
                    Enum.GetValues(typeof(EntityHaveReviewCount)).OfType<EntityHaveReviewCount>().ToList();

                var entityHaveReviewCountDisplayAttrList = new List<EntityHaveReviewCountDto>();

                foreach (var item in entityHaveReviewCountList)
                {
                    var entityHaveReviewCount = EnumHelper.GetDisplayForEnumValue(item);

                    entityHaveReviewCountDisplayAttrList.Add(new EntityHaveReviewCountDto()
                    {
                        Name = entityHaveReviewCount,
                        Value = (int)item
                    });
                }

                ViewBag.EntityHaveReviewCountList = entityHaveReviewCountDisplayAttrList;

                return View(command);
            }

            var footerCategory = _footerCategoryService.Get(command.Id).MapToEntity();

            _footerCategoryService.UpdateFooterCategory(footerCategory, command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "FooterCategoriesController", "Edit", "Success Edit Footer Category", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);

            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// تغییر وضعیت فعال بودن
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ChangeStatus(Guid id)
        {
            var footerCategory = _footerCategoryService.Get(id).MapToEntity();
            if (footerCategory == null)
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            footerCategory.IsActive = !footerCategory.IsActive;
            _footerCategoryService.Update(footerCategory);
            _footerCategoryService.Save();
            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// حذف دسته بندی
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var footerCategory = _footerCategoryService.Get(id);

                #region Remove relations

                var footerCategoryLinks = _footerCategoryLinkService.GetList(f => !f.IsDeleted);
                _footerCategoryLinkService.DeleteRange(footerCategoryLinks);
                _footerCategoryLinkService.Save();

                #endregion

                _footerCategoryService.Delete(footerCategory.Id);
                _footerCategoryService.Save();

                return Json(new
                {
                    Message = Strings.FooterCategoryModel_Delete_Successfully,
                    Success = Strings.Success,
                    Type = "success"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Message = Strings.Global_SystemError,
                    Success = Strings.Error,
                    Type = "error"
                });
            }
        }
    }
}