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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    public class ArticlesController : BaseController
    {
        private ArticleService _articleService;
        private ArticleTagService _articleTagService;
        private TagService _tagService;
        private AttachmentFileService _attachmentFileService;
        private LanguageService _languageService;
        private AuthorService _authorService;
        private EventLoger _eventLoger;

        public ArticlesController()
        {
            _articleService = new ArticleService();
            _articleTagService = new ArticleTagService();
            _tagService = new TagService();
            _attachmentFileService = new AttachmentFileService();
            _languageService = new LanguageService();
            _authorService = new AuthorService();
            _eventLoger = new EventLoger();
        }


        public ActionResult Index(int pageNumber = 1)
        {
            int pageSize = 5;

            var articles =
                _articleService.GetList(q => q.IsDeleted == false && q.CreatedBy == SessionData.Current.User.Id, a => a.OrderByDescending(o => o.CreatedDate),
                    q => q.ArticleTags, q => q.ArticleTags.Select(at => at.Tag));

            foreach (var article in articles)
            {
                article.AttachmentImageSource = article.AttachmentImageId.HasValue ? _attachmentFileService.GetAttachmentSourceValue(article.AttachmentImageId) : "/PhysicalStorage/no_image.png";
                article.AuthorName = article.AuthorId.HasValue ? _authorService.GetById(article.AuthorId.Value)?.FullName : "";
            }

            var pagedListArticles = articles.ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;
            
            TempData["PageNumber"] = pageNumber;
            TempData.Keep("PageNumber");

            return View(pagedListArticles);
        }

        public ActionResult Create()
        {
            ViewBag.Tags = _tagService.GetList(q =>
                q.IsDeleted == false && q.IsActive && q.ParentId != null && q.CreatedBy == SessionData.Current.User.Id);

            ViewBag.Languages = _languageService.GetList();

            ViewBag.Authors = _authorService.GetList(a => a.IsActive && !a.IsDeleted && a.CreatedBy == SessionData.Current.User.Id);

            return View(new ArticleModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ArticleModel model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {

                #region Article image

                if (file != null && file.ContentLength > 0)
                {
                    byte[] imageData = null;
                    var fileSize = file.ContentLength;
                    using (var binaryReader = new System.IO.BinaryReader(file.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(file.ContentLength);
                    }

                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    int imageWidth = img.Width;
                    int imageHeight = img.Height;

                    var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(),
                        System.IO.Path.GetFileName(file.FileName),
                        System.IO.Path.GetExtension(file.FileName), fileSize,
                        MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(file.FileName)),
                        imageWidth, imageHeight, "ArticleImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                    model.AttachmentImageId = attachmentId;
                }

                #endregion

                #region Article

                var article = new Article
                {
                    AuthorId = model.AuthorId,
                    Title = model.Title,
                    IsActive = model.IsActive,
                    IsDeleted = false,
                    CreatedBy = SessionData.Current.User.Id,
                    Source = model.Source,
                    Summary = model.Summary,
                    AttachmentImageId = model.AttachmentImageId,
                    LanguageId = model.LanguageId,
                    IsSpecial = model.IsSpecial,
                    IsSpecialCreatedDate = model.IsSpecial ? DateTime.Now : (DateTime?)null,
                    ImageCaption = model.ImageCaption,
                    ReviewCount = 0
                };
                _articleService.Insert(article);
                _articleService.Save();

                #endregion


                #region Tags

                model.Tags?.RemoveAll(t => t == Guid.Empty);
                _articleTagService.AssignTagsToArticle(article.Id, model.Tags, SessionData.Current.User.Id);

                #endregion

                _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id,
                    SessionData.Current.User.UserName, "ArticlesController", "Create", "Success Create Article",
                    HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);

                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1});
            }

            ViewBag.Tags = _tagService.GetList(q =>
                q.IsDeleted == false && q.IsActive && q.ParentId != null && q.CreatedBy == SessionData.Current.User.Id);

            ViewBag.Languages = _languageService.GetList();

            ViewBag.Authors = _authorService.GetList(a => a.IsActive && !a.IsDeleted && a.CreatedBy == SessionData.Current.User.Id);

            return View(model);
        }

        public ActionResult Edit(Guid Id)
        {
            var article = _articleService.GetList(q =>
                q.IsDeleted == false && q.Id == Id && q.CreatedBy == SessionData.Current.User.Id, null, q => q.ArticleTags).FirstOrDefault();

            if (article == null)
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            var articleModel = new ArticleModel
            {
                AuthorId = article.AuthorId,
                CreatedBy = article.CreatedBy,
                CreatedDate = article.CreatedDate,
                Id = article.Id,
                IsActive = article.IsActive,
                IsDeleted = article.IsDeleted,
                ModifiedBy = article.ModifiedBy,
                ModifiedDate = article.ModifiedDate,
                Title = article.Title,
                Summary = article.Summary,
                Source = article.Source,
                AttachmentImageId = article.AttachmentImageId,
                LanguageId = article.LanguageId,
                IsSpecial = article.IsSpecial,
                ImageCaption = article.ImageCaption,
            };

            #region FillTags

            ViewBag.Tags = _tagService.GetList(q =>
                q.IsDeleted == false && q.IsActive && q.ParentId != null && q.CreatedBy == SessionData.Current.User.Id);
            List<string> tagslist = new List<string>();
            foreach (var item in article.ArticleTagsDto)
            {
                tagslist.Add(item.TagId.ToString());
            }

            ViewBag.SelectedTags = string.Join(",", tagslist);

            #endregion

            ViewBag.Authors = _authorService.GetList(a => a.IsActive && !a.IsDeleted && a.CreatedBy == SessionData.Current.User.Id);

            ViewBag.Languages = _languageService.GetList();

            return View(articleModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ArticleModel model, HttpPostedFileBase file)
        {
            var article = _articleService
                .Get(q => q.IsDeleted == false && q.Id == model.Id && q.CreatedBy == SessionData.Current.User.Id)
                .MaptoEntity();
            if (ModelState.IsValid)
            {
                article.AuthorId = model.AuthorId;
                article.Title = model.Title;
                article.IsActive = model.IsActive;
                article.ModifiedBy = SessionData.Current.User.Id;
                article.ModifiedDate = DateTime.Now;
                article.Source = model.Source;
                article.Summary = model.Summary;
                article.LanguageId = model.LanguageId;
                article.AttachmentImageId = model.AttachmentImageId;

                if (!article.IsSpecial && model.IsSpecial)
                    article.IsSpecialCreatedDate = DateTime.Now;

                else if (article.IsSpecial && !model.IsSpecial)
                    article.IsSpecialCreatedDate = null;

                article.IsSpecial = model.IsSpecial;
                article.ImageCaption = model.ImageCaption;

                #region Images

                #region Article image

                var attachmentId = Guid.NewGuid();

                if (file != null && file.ContentLength > 0)
                {
                    byte[] imageData = null;
                    var fileSize = file.ContentLength;
                    using (var binaryReader = new System.IO.BinaryReader(file.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(file.ContentLength);
                    }

                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    int imageWidth = img.Width;
                    int imageHeight = img.Height;

                    attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, attachmentId,
                        System.IO.Path.GetFileName(file.FileName),
                        System.IO.Path.GetExtension(file.FileName), fileSize,
                        MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(file.FileName)),
                        imageWidth, imageHeight, "ArticleImage - " + model.Id, "", imageData, DateTime.Now);

                    article.AttachmentImageId = attachmentId;

                    if (attachmentId != null && attachmentId != Guid.Empty) //فایل ضمیمه جدید ذخیره شده
                    {
                        try
                        {
                            //فایل ضمیمه قبلی پاک میشود

                            article.AttachmentImageId = attachmentId;
                            _articleService.Update(article);
                            _articleService.Save();

                            if (model.AttachmentImageId.HasValue)
                            {
                                _attachmentFileService.RemoveAttachment(model.AttachmentImageId.Value);
                            }

                        }
                        catch (Exception ex)
                        {
                        }
                    }

                }

                #endregion


                #endregion

                _articleService.Update(article);
                _articleService.Save();

                #region Tags

                model.Tags?.RemoveAll(t => t == Guid.Empty);
                _articleTagService.AssignTagsToArticle(article.Id, model.Tags, SessionData.Current.User.Id);

                #endregion

                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
            }

            #region FillTags

            ViewBag.Tags = _tagService.GetList(q =>
                q.IsDeleted == false && q.IsActive && q.ParentId != null && q.CreatedBy == SessionData.Current.User.Id);
            List<string> tagslist = new List<string>();
            foreach (var item in article.ArticleTags)
            {
                tagslist.Add(item.TagId.ToString());
            }

            ViewBag.SelectedTags = string.Join(",", tagslist);

            #endregion

            ViewBag.Languages = _languageService.GetList();

            ViewBag.Authors = _authorService.GetList(a => a.IsActive && !a.IsDeleted && a.CreatedBy == SessionData.Current.User.Id);

            return View(model);
        }


        public ActionResult ChangeStatus(Guid articleId)
        {
            var article = _articleService.Get(a => a.Id == articleId).MaptoEntity();
            if (article == null)
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            article.IsActive = !article.IsActive;
            _articleService.Update(article);
            _articleService.Save();
            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        [HttpPost]
        public ActionResult Delete(Guid Id)
        {
            try
            {
                var article = _articleService.Get(a => a.Id == Id);


                #region Remove dependencies

                if (article.AttachmentImageId.HasValue)
                    _attachmentFileService.RemoveAttachment(article.AttachmentImageId.Value);

                var articleTags = _articleTagService.GetList(a => a.ArticleId == article.Id);
                foreach (var articleTag in articleTags)
                    _articleTagService.DeleteById(articleTag.Id);

                _articleTagService.Save();

                _articleService.DeleteById(article.Id);
                _articleService.Save();

                #endregion

                return Json(new
                {
                    Message = Strings.Delete_Article_Successfully,
                    Strings.Success,
                    Type = "success"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Message = Strings.Global_SystemError,
                    Strings.Error,
                    Type = "error"
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }


}