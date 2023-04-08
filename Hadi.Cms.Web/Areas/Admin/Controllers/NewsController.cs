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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hadi.Cms.ApplicationService.QueryModels;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    public class NewsController : BaseController
    {
        private NewsService _newsService;
        private AttachmentFileService _attachmentFileService;
        private LanguageService _languageService;
        private EventLoger _eventLoger;
        private NewsCategoryService _newsCategoryService;
        private NewsNewsCategoryService _newsNewsCategoryService;

        public NewsController()
        {
            _newsService = new NewsService();
            _attachmentFileService = new AttachmentFileService();
            _languageService = new LanguageService();
            _eventLoger = new EventLoger();
            _newsCategoryService = new NewsCategoryService();
            _newsNewsCategoryService = new NewsNewsCategoryService();
        }

        public ActionResult Index(int pageNumber = 1)
        {
            var pageSize = 5;

            var news = _newsService.GetList(q => !q.IsDeleted && q.CreatedBy == SessionData.Current.User.Id, null, n => n.NewsNewsCategories, n => n.NewsNewsCategories.Select(nc => nc.NewsCategory));

            ViewBag.PageNumber = pageNumber;

            return View(news.Select(n => new NewsDto()
            {
                Id = n.Id,
                Title = n.Title,
                ReleaseDate = n.ReleaseDate,
                IsActive = n.IsActive,
                IsPublished = n.IsPublished,
                CategoriesTitle = string.Join(" - ", n.NewsNewsCategories.Select(nc => nc.NewsCategoryDto.Title).ToList())
            }).OrderByDescending(n => n.CreatedDate).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            ViewBag.Languages = _languageService.GetList();
            ViewBag.CategoriesId = _newsCategoryService.GetList(null, o => o.OrderBy(c => c.OrderId));
            return View(new NewsModel());
        }

        [HttpPost]
        [ValidateInput(false)] // for validate news source html text
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewsModel model, List<HttpPostedFileBase> file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    foreach (var item in file)
                    {
                        if (item != null && item.ContentLength > 0)
                        {
                            var attachmentGuid = Guid.NewGuid();
                            byte[] imageData = null;
                            var fileSize = item.ContentLength;
                            using (var binaryReader = new System.IO.BinaryReader(item.InputStream))
                            {
                                imageData = binaryReader.ReadBytes(item.ContentLength);
                            }

                            var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                            int imageWidth = img.Width;
                            int imageHeight = img.Height;

                            attachmentGuid = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, attachmentGuid, System.IO.Path.GetFileName(item.FileName),
                                System.IO.Path.GetExtension(item.FileName), fileSize, Hadi.Cms.Infrastructure.Helpers.MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(item.FileName)),
                                imageWidth, imageHeight, "newsImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                            if (file.IndexOf(item) == 0)//ThumbnailImage
                            {
                                model.ThumbnailImage = attachmentGuid;
                            }
                            else if (file.IndexOf(item) == 1)//NewsImage
                            {
                                model.Image = attachmentGuid;
                            }
                            else if (file.IndexOf(item) == 2)//MainTitrImage
                            {
                                model.MainTitrImage = attachmentGuid;
                            }
                        }
                    }
                    model.Id = Guid.NewGuid();
                    var news = new News
                    {
                        Id = model.Id,
                        Image = model.Image,
                        IsHotLink = model.IsHotLink,
                        IsLiveNews = model.IsLiveNews,
                        IsMainTitr = model.IsMainTitr,
                        IsPublished = model.IsPublished,
                        MainTitrImage = model.MainTitrImage,
                        ReleaseDate = DateTime.Parse(model.ReleaseDate),
                        RuTitr = model.RuTitr,
                        ShowPriorityDate = DateTime.Parse(model.ShowPriorityDate),
                        Source = model.Source,
                        SubTitle = model.SubTitle,
                        ThumbnailImage = model.ThumbnailImage,
                        Title = model.Title,
                        WithFilm = model.WithFilm,
                        WithImage = model.WithImage,
                        WithVoice = model.WithVoice,
                        LanguageId = model.LanguageId,
                        CreatedBy = SessionData.Current.User.Id,
                        IsDeleted = false,
                        IsActive = model.IsActive
                    };

                    _newsService.Insert(news);
                    _newsService.Save();

                    // افزودن دسته بندی به خبر
                    model.CategoriesId?.RemoveAll(c => c == Guid.Empty);
                    _newsNewsCategoryService.AssignCategoriesToNews(news.Id, model.CategoriesId,
                        SessionData.Current.User.Id);

                    _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "NewsController", "Create", "Success Create News", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);

                    return RedirectToAction("Index");
                }
            }
            ViewBag.Languages = _languageService.GetList();
            ViewBag.CategoriesId = _newsCategoryService.GetList(null, o => o.OrderBy(c => c.OrderId));
            return View(model);
        }

        public ActionResult Edit(Guid NewsId)
        {
            var model = new NewsModel();
            var news = _newsService.Get(q => !q.IsDeleted && q.CreatedBy == SessionData.Current.User.Id && q.Id == NewsId);

            model.Id = news.Id;
            model.Title = news.Title;
            model.IsHotLink = news.IsHotLink;
            model.IsLiveNews = news.IsLiveNews;
            model.IsMainTitr = news.IsMainTitr;
            model.IsPublished = news.IsPublished;
            model.LanguageId = news.LanguageId;
            model.CreatedBy = news.CreatedBy;
            model.CreatedDate = news.CreatedDate;
            model.IsActive = news.IsActive;
            model.IsDeleted = news.IsDeleted;
            model.ModifiedBy = news.ModifiedBy;
            model.ModifiedDate = news.ModifiedDate;


            // change date format to gregorian
            var gregorianReleaseDate = new DateTime();
            var gregorianShowPriorityDate = new DateTime();

            var pCalendar = new System.Globalization.PersianCalendar();
            gregorianReleaseDate = pCalendar.ToDateTime(news.ReleaseDate.Year, news.ReleaseDate.Month,
                news.ReleaseDate.Day, news.ReleaseDate.Hour, news.ReleaseDate.Minute, news.ReleaseDate.Second, 0);

            gregorianShowPriorityDate = pCalendar.ToDateTime(news.ShowPriorityDate.Year, news.ShowPriorityDate.Month,
                news.ShowPriorityDate.Day, news.ShowPriorityDate.Hour, news.ShowPriorityDate.Minute, news.ShowPriorityDate.Second, 0);
            // -------

            model.ReleaseDate = gregorianReleaseDate.ToString();
            model.ShowPriorityDate = gregorianShowPriorityDate.ToString();

            model.RuTitr = news.RuTitr;
            model.Source = news.Source;
            model.SubTitle = news.SubTitle;
            model.WithFilm = news.WithFilm;
            model.WithImage = news.WithImage;
            model.WithVoice = news.WithVoice;

            ViewBag.NewsId = NewsId;

            var attachmentThumbnail = _attachmentFileService.Get(a => a.Id == news.ThumbnailImage);
            var attachmentNews = _attachmentFileService.Get(a => a.Id == news.Image);
            var attachmentMainTitr = _attachmentFileService.Get(a => a.Id == news.MainTitrImage);

            if (attachmentThumbnail != null)
            {
                ViewBag.ThumbnailAttachmentName = attachmentThumbnail.Name;
                ViewBag.ThumbnailAttachmentGuid = attachmentThumbnail.Id;
                model.ThumbnailImage = attachmentThumbnail.Id;
            }
            if (attachmentNews != null)
            {
                ViewBag.NewsAttachmentName = attachmentNews.Name;
                ViewBag.NewsAttachmentGuid = attachmentNews.Id;
                model.Image = attachmentNews.Id;
            }
            if (attachmentMainTitr != null)
            {
                ViewBag.MainTitrAttachmentName = attachmentMainTitr.Name;
                ViewBag.MainTitrAttachmentGuid = attachmentMainTitr.Id;
                model.MainTitrImage = attachmentMainTitr.Id;
            }

            ViewBag.Languages = _languageService.GetList();
            ViewBag.CategoriesId = _newsCategoryService.GetList(null, o => o.OrderBy(c => c.OrderId));
            return View(model);
        }


        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NewsModel model, List<HttpPostedFileBase> file)
        {
            if (ModelState.IsValid)
            {
                Guid? thumbnailAttachmentGuid = null;
                Guid? newsAttachmentGuid = null;
                Guid? mainTitrAttachmentGuid = null;

                var existNews = _newsService.Get(q => !q.IsDeleted && q.CreatedBy == SessionData.Current.User.Id && q.Id == model.Id);

                model.ThumbnailImage = existNews.ThumbnailImage;
                model.Image = existNews.Image;
                model.MainTitrImage = existNews.MainTitrImage;

                if (file != null)
                {
                    foreach (var item in file)
                    {
                        if (item != null && item.ContentLength > 0)
                        {
                            var attachmentGuid = Guid.NewGuid();
                            byte[] imageData = null;
                            var fileSize = item.ContentLength;
                            using (var binaryReader = new System.IO.BinaryReader(item.InputStream))
                            {
                                imageData = binaryReader.ReadBytes(item.ContentLength);
                            }

                            var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                            int imageWidth = img.Width;
                            int imageHeight = img.Height;

                            attachmentGuid = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, attachmentGuid, System.IO.Path.GetFileName(item.FileName),
                                 System.IO.Path.GetExtension(item.FileName), fileSize, Infrastructure.Helpers.MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(item.FileName)),
                                 imageWidth, imageHeight, "newsImage - " + model.Id, "", imageData, DateTime.Now);

                            if (file.IndexOf(item) == 0)//ThumbnailImage
                            {
                                thumbnailAttachmentGuid = model.ThumbnailImage;
                                model.ThumbnailImage = attachmentGuid;
                            }
                            else if (file.IndexOf(item) == 1)//NewsImage
                            {
                                newsAttachmentGuid = model.Image;
                                model.Image = attachmentGuid;
                            }
                            else if (file.IndexOf(item) == 2)//MainTitrImage
                            {
                                mainTitrAttachmentGuid = model.MainTitrImage;
                                model.MainTitrImage = attachmentGuid;
                            }
                        }
                    }
                }
                model.ShowPriorityDate = DateTime.Parse(model.ShowPriorityDate).ToString();
                model.ReleaseDate = DateTime.Parse(model.ReleaseDate).ToString();
                var news = new News
                {
                    Id = model.Id,
                    Image = model.Image,
                    IsHotLink = model.IsHotLink,
                    IsLiveNews = model.IsLiveNews,
                    IsMainTitr = model.IsMainTitr,
                    IsPublished = model.IsPublished,
                    MainTitrImage = model.MainTitrImage,
                    ReleaseDate = DateTime.Parse(model.ReleaseDate),
                    RuTitr = model.RuTitr,
                    ShowPriorityDate = DateTime.Parse(model.ShowPriorityDate),
                    Source = model.Source,
                    SubTitle = model.SubTitle,
                    ThumbnailImage = model.ThumbnailImage,
                    Title = model.Title,
                    WithFilm = model.WithFilm,
                    WithImage = model.WithImage,
                    WithVoice = model.WithVoice,
                    LanguageId = model.LanguageId,
                    IsActive = model.IsActive,
                    CreatedBy = existNews.CreatedBy,
                    CreatedDate = existNews.CreatedDate,
                    IsDeleted = existNews.IsDeleted,
                    ModifiedBy = SessionData.Current.User.Id,
                    ModifiedDate = DateTime.Now
                };

                _newsService.Update(news);
                _newsService.Save();

                // افزودن دسته بندی به خبر
                model.CategoriesId?.RemoveAll(c => c == Guid.Empty);
                _newsNewsCategoryService.AssignCategoriesToNews(news.Id, model.CategoriesId,
                    SessionData.Current.User.Id);

                if (thumbnailAttachmentGuid != null) //فایل ضمیمه جدید ذخیره شده
                {
                    try
                    {
                        //فایل ضمیمه قبلی پاک میشود
                        _attachmentFileService.RemoveAttachment(thumbnailAttachmentGuid ?? Guid.NewGuid());
                    }
                    catch { }
                }
                if (newsAttachmentGuid != null) //فایل ضمیمه جدید ذخیره شده
                {
                    try
                    {
                        //فایل ضمیمه قبلی پاک میشود
                        _attachmentFileService.RemoveAttachment(newsAttachmentGuid ?? Guid.NewGuid());
                    }
                    catch { }
                }
                if (mainTitrAttachmentGuid != null) //فایل ضمیمه جدید ذخیره شده
                {
                    try
                    {
                        //فایل ضمیمه قبلی پاک میشود
                        _attachmentFileService.RemoveAttachment(mainTitrAttachmentGuid ?? Guid.NewGuid());
                    }
                    catch { }
                }

                //if (result)
                //{
                //    ViewBag.Message = "ویرایش انجام شد.";
                //    ViewBag.Success = "1";
                //}
                //else
                //{
                //    ViewBag.Message = "خطا در ثبت اطلاعات!";
                //    ViewBag.Success = "0";
                //}

                return RedirectToAction("Index");
            }

            ViewBag.Languages = _languageService.GetList();
            ViewBag.CategoriesId = _newsCategoryService.GetList(null, o => o.OrderBy(c => c.OrderId));
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(Guid NewsId)
        {
            try
            {
                var news = _newsService.Get(q => !q.IsDeleted && q.CreatedBy == SessionData.Current.User.Id && q.Id == NewsId);

                if (news != null)
                {
                    if (news.ThumbnailImage != null) //فایل ضمیمه جدید ذخیره شده
                    {
                        try
                        {
                            //فایل ضمیمه قبلی پاک میشود
                            _attachmentFileService.RemoveAttachment(news.ThumbnailImage.Value);
                        }
                        catch { }
                    }
                    if (news.Image != null) //فایل ضمیمه جدید ذخیره شده
                    {
                        try
                        {
                            //فایل ضمیمه قبلی پاک میشود
                            _attachmentFileService.RemoveAttachment(news.Image.Value);
                        }
                        catch { }
                    }
                    if (news.MainTitrImage != null) //فایل ضمیمه جدید ذخیره شده
                    {
                        try
                        {
                            //فایل ضمیمه قبلی پاک میشود
                            _attachmentFileService.RemoveAttachment(news.MainTitrImage.Value);
                        }
                        catch { }
                    }

                    news.IsActive = false;
                    news.IsDeleted = true;
                    _newsService.Update(news.MaptoEntity());
                    _newsService.Save();

                    ViewBag.Message = Strings.News_Delete_Success;
                    ViewBag.Success = Strings.Success;
                }
                else
                {
                    ViewBag.Message = Strings.News_Delete_Dont_Have_Permission;
                    ViewBag.Success = Strings.Error;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = Strings.News_Delete_Error;
                ViewBag.Success = @Strings.Error;
            }

            return Json(new
            {
                Message = ViewBag.Message,
                Success = ViewBag.Success
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ChangePublishStatus(Guid NewsId)
        {
            try
            {
                var news = _newsService.Get(q => q.Id == NewsId).MaptoEntity();

                if (news != null)
                {
                    news.IsPublished = !news.IsPublished;
                    _newsService.Update(news);
                    _newsService.Save();

                    ViewBag.Success = "ok";
                }
                else
                {
                    ViewBag.Success = "nok";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Success = "nok";
            }

            return Json(new
            {
                Success = ViewBag.Success
            }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult NewsContent(Guid newsId)
        {
            var news = _newsService.GetById(newsId);

            if (SessionData.Current.CurrentUserIsAuthenticate)
            {
                return View(news);
            }
            else
            {
                return RedirectToAction("Details", "News", new { area = "", Id = news.Id });
            }
        }
    }
}