using PagedList;
using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Infrastructure.Helpers;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Language.Resources;
using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Web.Utilities;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// مدیریت نویسندگان
    /// </summary>
    public class AuthorsController : Controller
    {
        private readonly AuthorService _authorService;
        private readonly ArticleService _articleService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly EventLoger _eventLoger;

        public AuthorsController()
        {
            _authorService = new AuthorService();
            _articleService = new ArticleService();
            _attachmentFileService = new AttachmentFileService();
            _eventLoger = new EventLoger();
        }

        /// <summary>
        /// دریافت لیست نویسندگان
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public ActionResult Index(int pageNumber = 1)
        {
            var pageSize = 5;
            var authors = _authorService.GetList(a => !a.IsDeleted && a.CreatedBy == SessionData.Current.User.Id, a => a.OrderByDescending(o => o.CreatedDate));
            
            foreach (var author in authors)
                author.AuthorImageSource = author.AuthorImageGuid.HasValue ? _attachmentFileService.GetAttachmentSourceValue(author.AuthorImageGuid) : "/PhysicalStorage/no_image.png";

            var pagedList = authors.ToPagedList(pageNumber, pageSize);
            ViewBag.PageNumber = pageNumber;
            
            TempData["PageNumber"] = pageNumber;
            TempData.Keep("PageNumber");

            return View(pagedList);
        }

        /// <summary>
        /// صفحه ی ثبت نویسنده
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View(new AuthorCreateCommand());
        }

        /// <summary>
        /// ثبت نویسنده جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="authorImage"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AuthorCreateCommand command, HttpPostedFileBase authorImage)
        {
            if (!ModelState.IsValid)
                return View(command);

            var existsAuthor = _authorService.Any(a => a.FullName == command.FullName);
            if (existsAuthor)
            {
                ModelState.AddModelError("FullName", Strings.Author_AuthorExists);
                return View(command);
            }

            #region Insert author image

            if (authorImage != null && authorImage.ContentLength > 0)
            {
                byte[] imageData;
                var fileSize = authorImage.ContentLength;

                using (var binaryReader = new System.IO.BinaryReader(authorImage.InputStream))
                    imageData = binaryReader.ReadBytes(authorImage.ContentLength);

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(authorImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(authorImage.FileName),
                    System.IO.Path.GetExtension(authorImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "AuthorImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.AuthorImageGuid = attachmentId;
            }

            #endregion

            _authorService.CreateNewAuthor(command, SessionData.Current.User.Id);

            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "AuthorsController", "Create", "Success Create Author", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);

            return RedirectToAction("Index" , new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1});
        }

        /// <summary>
        /// صفحه ی ویرایش اطلاعات نویسنده
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {
            var author = _authorService.GetById(id);
            if (author == null)
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            return View(new AuthorEditCommand
            {
                Id = id,
                FullName = author.FullName,
                InstagramAddress = author.InstagramAddress,
                TelegramAddress = author.TelegramAddress,
                LinkedInAddress = author.LinkedInAddress,
                AuthorImageSource = _attachmentFileService.GetAttachmentSourceValue(author.AuthorImageGuid),
                AuthorImageGuid = author.AuthorImageGuid
            });
        }

        /// <summary>
        /// ویرایش اطلاعات نویسنده
        /// </summary>
        /// <param name="command"></param>
        /// <param name="authorImage"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AuthorEditCommand command, HttpPostedFileBase authorImage)
        {
            if (!ModelState.IsValid)
                return View(command);

            var existsRecord = _authorService.Any(a => a.Id == command.Id && a.IsActive && !a.IsDeleted && a.CreatedBy == SessionData.Current.User.Id);
            if (!existsRecord)
                return HttpNotFound("No author found .");

            var existsAuthor = _authorService.Any(a => a.Id != command.Id && a.FullName == command.FullName);
            if (existsAuthor)
            {
                ModelState.AddModelError("FullName", Strings.Author_AuthorExists);
                return View(command);
            }

            var author = _authorService.GetById(command.Id).MapToEntity();

            #region Insert author image

            if (authorImage != null && authorImage.ContentLength > 0)
            {
                if (author.AuthorImageGuid.HasValue) { }
                _attachmentFileService.RemoveAttachment(author.AuthorImageGuid.Value);


                byte[] imageData;
                var fileSize = authorImage.ContentLength;

                using (var binaryReader = new System.IO.BinaryReader(authorImage.InputStream))
                    imageData = binaryReader.ReadBytes(authorImage.ContentLength);

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(authorImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(authorImage.FileName),
                    System.IO.Path.GetExtension(authorImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "AuthorImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.AuthorImageGuid = attachmentId;

            }

            #endregion

            _authorService.EditAuthorInformation(author, command, SessionData.Current.User.Id);

            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "AuthorsController", "Edit", "Success Edit Author", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);

            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// حذف نویسنده
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var author = _authorService.GetById(id);
                if (author == null)
                {
                    return Json(new
                    {
                        Message = Strings.DeleteOperationFailed,
                        Success = Strings.Error,
                        Type = "error"
                    });
                }

                var articles = _articleService.GetList(a => a.AuthorId == author.Id && a.CreatedBy == SessionData.Current.User.Id);

                foreach (var article in articles)
                    _articleService.DeleteById(article.Id);

                _articleService.Save();

                _authorService.DeleteById(author.Id);
                _authorService.Save();

                return Json(new
                {
                    Message = Strings.Auhtor_AuhtorDeleteSuccessfully,
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