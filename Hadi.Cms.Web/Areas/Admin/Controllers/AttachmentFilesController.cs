using PagedList;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Infrastructure.Helpers;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Web.Controllers;
using Hadi.Cms.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.Language.Resources;
using Hadi.Cms.Model.QueryModels;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// مدیریت فایل ها
    /// </summary>
    public class AttachmentFilesController : BaseController
    {
        private readonly AttachmentFileService _attachmentFileService;
        private readonly EventLoger _eventLoger;
        private readonly UserService _userService;
        private readonly AttachmentFileTagService _attachmentFileTagService;
        private readonly TagService _tagService;
        private readonly ArticleService _articleService;
        private readonly MailService _mailService;

        public AttachmentFilesController()
        {
            _attachmentFileService = new AttachmentFileService();
            _userService = new UserService();
            _eventLoger = new EventLoger();
            _attachmentFileTagService = new AttachmentFileTagService();
            _tagService = new TagService();
            _articleService = new ArticleService();
            _mailService = new MailService();
        }

        /// <summary>
        /// دریافت لیست فایل ها
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public ActionResult Index(int pageNumber = 1)
        {
            int pageSize = 5;


            var attachmentFiles = _attachmentFileService
                .GetList(null, o => o.OrderByDescending(c => c.LastModified), a => a.AttachmentFileTags,
                    a => a.AttachmentFileTags.Select(at => at.Tag)).ToList();

            foreach (var item in attachmentFiles)
            {
                item.CreatedByUserName = item.CreatedBy != null && item.CreatedBy != Guid.Empty ?
                    _userService.Get(q => q.Id == item.CreatedBy && !q.IsDeleted).FullName : "";

                if (!string.IsNullOrEmpty(item.PhysicalPath))
                    item.ShowImage = item.PhysicalPath;

                else
                    item.ShowImage = "/Attachments/GetAttachment/" + item.Id;

                if (!item.MimeType.Contains("image"))
                    item.ShowImage = "/PhysicalStorage/attach.png";

                if (item.IsPhysicalStorage && string.IsNullOrEmpty(item.PhysicalPath))
                    item.ShowImage = "/PhysicalStorage/no_image.png";
            }

            var pagesListAttachmentFiles = attachmentFiles.OrderByDescending(o => o.LastModified).ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;

            return View(pagesListAttachmentFiles);
        }

        /// <summary>
        /// صفحه ی ثبت فایل جدید
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ViewBag.Tags = _tagService.GetList(t => t.IsActive && !t.IsDeleted && t.ParentId != null);
            return View(new AttachmentFileModel());
        }

        /// <summary>
        /// ثبت فایل جدید
        /// </summary>
        /// <param name="tagsId"></param>
        /// <param name="posterImageGuid"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromBody] List<Guid> tagsId, Guid? posterImageGuid, HttpPostedFileBase file)
        {
            var attachmentId = new Guid();

            if (file != null)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string mimeType = MimeTypeHelper.GetMimeType(Path.GetExtension(file.FileName));

                    byte[] fileData = null;
                    var fileSize = file.ContentLength;
                    using (var binaryReader = new BinaryReader(file.InputStream))
                    {
                        fileData = binaryReader.ReadBytes(file.ContentLength);
                    }

                    int imageWidth = 0;
                    int imageHeight = 0;

                    if (mimeType.Contains("image"))
                    {
                        var img = Image.FromStream(new MemoryStream(fileData));
                        imageWidth = img.Width;
                        imageHeight = img.Height;
                    }

                    attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), Path.GetFileName(file.FileName),
                        Path.GetExtension(file.FileName), fileSize, MimeTypeHelper.GetMimeType(Path.GetExtension(file.FileName)),
                        imageWidth, imageHeight, "AttachmentFile" + " - " + Guid.NewGuid(), "", fileData, DateTime.Now, posterImageGuid);

                    // نسبت دادن تگ به فایل پیوست شده
                    tagsId.RemoveAll(t => t == Guid.Empty);
                    if (tagsId.Count > 0)
                    {
                        _attachmentFileTagService.AssignTagsToAttachmentFile(attachmentId, tagsId, SessionData.Current.User.Id);
                    }
                }
            }

            ViewBag.Tags = _tagService.GetList(t => t.IsActive && !t.IsDeleted && t.ParentId != null);

            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "FileManagerController", "Create", "Success Create File", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// صفحه ی ویرایش تگ های فایل
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpGet] 
        public ActionResult EditTags(Guid id)
        {
            var attachmentFile = _attachmentFileService.GetList(a => a.Id == id, null, a => a.AttachmentFileTags,
                a => a.AttachmentFileTags.Select(at => at.Tag)).FirstOrDefault();

            if (attachmentFile == null)
                return RedirectToAction("Index");

            #region FillTags
            ViewBag.Tags = _tagService.GetList(t => t.IsActive && !t.IsDeleted && t.ParentId != null && t.CreatedBy == SessionData.Current.User.Id);
            List<string> tagslist = new List<string>();
            foreach (var item in attachmentFile.AttachmentFileTagDto)
            {
                tagslist.Add(item.TagId.ToString());
            }
            ViewBag.SelectedTags = string.Join(",", tagslist);
            #endregion

            return View(new EditAttachmentFileTagCommand()
            {
                AttachmentId = attachmentFile.Id,
            });
        }

        /// <summary>
        /// ویرایش تگ هاب فایل
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTags([FromBody] EditAttachmentFileTagCommand command)
        {
            command.Tags.RemoveAll(t => t == Guid.Empty);
            _attachmentFileTagService.AssignTagsToAttachmentFile(command.AttachmentId, command.Tags, SessionData.Current.User.Id);
            return RedirectToAction("Index");
        }


        /// <summary>
        /// حذف فایل
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public ActionResult Delete(Guid Id)
        {
            try
            {
                var attachmentFile = _attachmentFileService.Get(q => q.Id == Id, q => q.AttachmentFileTags);

                if (attachmentFile != null)
                {
                    //فایل ضمیمه پاک میشود
                    if (System.IO.File.Exists(attachmentFile.PhysicalPath))
                        System.IO.File.Delete(attachmentFile.PhysicalPath);

                    #region remove dependency
                    var attachmentFileTags = _attachmentFileTagService.GetList(at => at.AttachmentFileId == Id);

                    foreach (var attachmentFileTag in attachmentFileTags)
                        _attachmentFileTagService.DeleteById(attachmentFileTag.Id);

                    _attachmentFileTagService.Save();


                    foreach (var article in attachmentFile.ArticlesDto)
                        article.AttachmentImageId = null;

                    _articleService.Save();

                    foreach (var mail in attachmentFile.MailAttachmentsDto)
                        _mailService.DeleteById(mail.Id);

                    _mailService.Save();
                    #endregion

                    _attachmentFileService.RemoveAttachment(attachmentFile.Id);
                    ViewBag.Message = Strings.File_Managment_Delete_Successfully;
                    ViewBag.Type = "success";
                    ViewBag.Success = Strings.Success;
                }
                else
                {
                    ViewBag.Message = Strings.File_Managment_Dont_Permission_For_Delete;
                    ViewBag.Success = Strings.Error;
                    ViewBag.Type = "success";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = Strings.File_Managment_Error_While_Delete_File;
                ViewBag.Success = Strings.Error;
                ViewBag.Type = "error";
            }

            return Json(new
            {
                Message = ViewBag.Message,
                Success = ViewBag.Success,
                Type = ViewBag.Type
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadFile(Guid Id)
        {
            var attachmentFile = _attachmentFileService.Get(q => q.Id == Id);
            byte[] byteImage = null;
            if (attachmentFile.IsBinaryStorage)
                byteImage = attachmentFile.Binary;

            return File(byteImage, attachmentFile.MimeType);
        }
    }
}