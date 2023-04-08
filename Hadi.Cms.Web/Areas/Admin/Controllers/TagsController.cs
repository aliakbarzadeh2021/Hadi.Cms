using PagedList;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Model.QueryModels;
using Hadi.Cms.Web.Controllers;
using Hadi.Cms.Web.Utilities;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    public class TagsController : BaseController
    {
        private readonly TagService _tagService;
        private readonly EventLoger _eventLoger;
        private readonly ArticleTagService _articleTagService;
        private readonly CourseTagService _courseTagService;
        private readonly ServiceTagService _serviceTagService;

        public TagsController()
        {
            _tagService = new TagService();
            _eventLoger = new EventLoger();
            _articleTagService = new ArticleTagService();
            _courseTagService = new CourseTagService();
            _serviceTagService = new ServiceTagService();
        }

        public ActionResult Index(int pageNumber = 1)
        {
            int pageSize = 5;

            var tags = _tagService.GetList(q => q.IsDeleted == false && q.CreatedBy == SessionData.Current.User.Id,q => q.OrderByDescending(o => o.CreatedDate));


            foreach (var tag in tags)
                tag.ParentTitle = _tagService.Get(t => t.Id == tag.ParentId)?.Title;

            var pagedListTags = tags.ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;

            return View(pagedListTags);
        }

        public ActionResult Create()
        {
            ViewBag.Tags = _tagService.GetList(t => t.IsActive && !t.IsDeleted && t.CreatedBy == SessionData.Current.User.Id);

            return View(new TagModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TagModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = SessionData.Current.User.Id;
                var currentDateTime = DateTime.Now;
                var tag = new Tag
                {
                    Title = model.Title,
                    IsActive = model.IsActive,
                    IsDeleted = false,
                    CreatedBy = userId,
                    ModifiedBy = userId,
                    ParentId = model.ParentId,
                    Color = model.Color,
                    CreatedDate = currentDateTime,
                    ModifiedDate = currentDateTime,
                    UniqueValue = model.UniqueValue,
                };
                _tagService.Insert(tag);
                _tagService.Save();

                _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "TagsController", "Create", "Success Create Tag", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);

                return RedirectToAction("Index");
            }

            ViewBag.Tags = _tagService.GetList(t => t.IsActive && !t.IsDeleted && t.CreatedBy == SessionData.Current.User.Id);

            return View(model);
        }

        public ActionResult Edit(Guid Id)
        {
            var tag = _tagService.Get(q => q.IsDeleted == false && q.Id == Id && q.CreatedBy == SessionData.Current.User.Id);
            if (tag == null)
                return RedirectToAction("Index");

            ViewBag.Tags = _tagService.GetList(t => t.IsActive && !t.IsDeleted && t.Id != tag.Id && t.CreatedBy == SessionData.Current.User.Id);

            var tagModel = new TagModel
            {
                CreatedBy = tag.CreatedBy,
                CreatedDate = tag.CreatedDate,
                Id = tag.Id,
                IsActive = tag.IsActive,
                IsDeleted = tag.IsDeleted,
                ModifiedBy = tag.ModifiedBy,
                ModifiedDate = tag.ModifiedDate,
                Title = tag.Title,
                ParentId = tag.ParentId,
                Color = tag.Color,
                UniqueValue = tag.UniqueValue,
            };

            return View(tagModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TagModel model)
        {
            var tag = _tagService.Get(q => q.IsDeleted == false && q.Id == model.Id && q.CreatedBy == SessionData.Current.User.Id).MaptoEntity();

            if (ModelState.IsValid)
            {
                tag.Title = model.Title;
                tag.IsActive = model.IsActive;
                tag.ModifiedBy = SessionData.Current.User.Id;
                tag.ModifiedDate = DateTime.Now;
                tag.ParentId = model.ParentId;
                tag.Color = model.Color;
                tag.UniqueValue = model.UniqueValue;

                _tagService.Update(tag);
                _tagService.Save();

                return RedirectToAction("Index");
            }

            ViewBag.Tags = _tagService.GetList(t => t.IsActive && !t.IsDeleted && t.Id != tag.Id && t.CreatedBy == SessionData.Current.User.Id);

            return View(model);
        }

        public ActionResult ChangeStatus(Guid tagId)
        {
            var tag = _tagService.GetById(tagId).MaptoEntity();
            if (tag == null)
                return RedirectToAction("Index");

            tag.IsActive = !tag.IsActive;
            _tagService.Update(tag);
            _tagService.Save();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(Guid Id)
        {
            try
            {
                var tag = _tagService.Get(q => q.IsDeleted == false && q.Id == Id && q.CreatedBy == SessionData.Current.User.Id).MaptoEntity();

                if (tag != null)
                {
                    // Remove tags from articles
                    var articleTags = _articleTagService.GetList(at => at.TagId == tag.Id);
                    foreach (var articleTag in articleTags)
                    {
                        _articleTagService.DeleteById(articleTag.MaptoEntity().Id);
                    }
                    _articleTagService.Save();

                    //Remove tags from courses
                    var courseTags = _courseTagService.GetList(c => c.TagId == tag.Id);
                    foreach (var courseTag in courseTags)
                    {
                        _courseTagService.Delete(courseTag.Id);
                    }
                    _courseTagService.Save();

                    // Remove tag from service
                    var serviceTags = _serviceTagService.GetList(s => s.TagId == tag.Id);
                    foreach (var serviceTag in serviceTags)
                    {
                        _serviceTagService.Delete(serviceTag.Id);
                    }
                    _serviceTagService.Save();

                    _tagService.DeleteById(tag.Id);
                    _tagService.Save();

                    ViewBag.Message = "برچسب انتخاب شده با موفقیت حذف شد.";
                    ViewBag.Success = "حذف شد!";
                }
                else
                {
                    ViewBag.Message = "شما مجوز حذف این برچسب را ندارید.";
                    ViewBag.Success = "خطا!";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "خطا در حذف برچسب";
                ViewBag.Success = "خطا!";
            }

            return Json(new
            {
                Message = ViewBag.Message,
                Success = ViewBag.Success
            }, JsonRequestBehavior.AllowGet);
        }


    }
}