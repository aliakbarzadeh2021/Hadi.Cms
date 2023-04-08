using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Infrastructure.Helpers;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Language.Resources;
using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Web.Utilities;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// مدیریت متدولوژی های دپارتمان
    /// </summary>
    public class MethodologiesController : Controller
    {
        private readonly MethodologyService _methodologyService;
        private readonly EventLoger _eventLoger;
        private readonly AttachmentFileService _attachmentFileService;
        public MethodologiesController()
        {
            _methodologyService = new MethodologyService();
            _attachmentFileService = new AttachmentFileService();
            _eventLoger = new EventLoger();
        }

        /// <summary>
        /// دریافت لیست تکنولوژی های دپارتمان
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public ActionResult Index(Guid id, int pageNumber = 1)
        {
            var pagedSize = 5;
            var methodologies = _methodologyService.GetList(m => !m.IsDeleted && m.DepartmentId == id, o => o.OrderByDescending(m => m.CreatedDate), m => m.Department);

            foreach (var methodology in methodologies)
                methodology.ImageSource = methodology.ImageAttachmentId.HasValue ?
                    _attachmentFileService.GetAttachmentSourceValue(methodology.ImageAttachmentId) : "/PhysicalStorage/no_image.png";

            var pagedList = methodologies.ToPagedList(pageNumber, pagedSize);
            ViewBag.DepartmentId = id;
            ViewBag.PageNumber = pageNumber;

            TempData["PageNumber"] = pageNumber;
            TempData.Keep("PageNumber");
            TempData["Id"] = id;
            TempData.Keep("Id");

            return View(pagedList);
        }

        /// <summary>
        /// صفحه ی ثبت متدولوژی
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(Guid id)
        {
            return View(new MethodologyCreateCommand
            {
                DepartmentId = id
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MethodologyCreateCommand command, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
                return View(command);

            if (file != null && file.ContentLength > 0)
            {
                byte[] imageData;
                var fileSize = file.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(file.InputStream))
                {
                    imageData = binaryReader.ReadBytes(file.ContentLength);
                }

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
                    imageWidth, imageHeight, "MethodologyImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.ImageAttachmentId = attachmentId;
            }

            _methodologyService.CreateNewTechnology(command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "MethodologiesController", "Create", "Success Create Methodology", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { id = Guid.Parse(TempData["Id"].ToString()), pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// صفحه ی ویرایش متدولوژی
        /// </summary>
        /// <param name="id"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id, Guid departmentId)
        {
            var methodology = _methodologyService.Get(id).MapToEntity();
            if (methodology == null)
                return RedirectToAction("Index", new { id = Guid.Parse(TempData["Id"].ToString()), pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            return View(new MethodologyEditCommand
            {
                Id = methodology.Id,
                DepartmentId = methodology.DepartmentId,
                Title = methodology.Title,
                Description = methodology.Description,
                ImageAttachmentId = methodology.ImageAttachmentId,
                IsActive = methodology.IsActive,
                ImageSource = _attachmentFileService.GetAttachmentSourceValue(methodology.ImageAttachmentId)
            });
        }

        /// <summary>
        /// ویرایش متدولوژی
        /// </summary>
        /// <param name="command"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MethodologyEditCommand command, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
                return View(command);

            var methodology = _methodologyService.Get(command.Id).MapToEntity();

            if (file != null && file.ContentLength > 0)
            {
                if (methodology.ImageAttachmentId.HasValue)
                    _attachmentFileService.RemoveAttachment(methodology.ImageAttachmentId.Value);

                byte[] imageData;
                var fileSize = file.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(file.InputStream))
                {
                    imageData = binaryReader.ReadBytes(file.ContentLength);
                }

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
                    imageWidth, imageHeight, "MethodologyImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.ImageAttachmentId = attachmentId;
            }

            _methodologyService.UpdateMethodology(methodology, command, SessionData.Current.User.Id);
            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "MethodologiesController", "Edit", "Success Edit Methodology", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { id = Guid.Parse(TempData["Id"].ToString()), pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// تغییر وضعیت فعال بودن متدلوژی
        /// </summary>
        /// <param name="id"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public ActionResult ChangeStatus(Guid id, Guid departmentId)
        {
            var methodology = _methodologyService.Get(id).MapToEntity();
            if (methodology == null)
                return RedirectToAction("Index", new { id = Guid.Parse(TempData["Id"].ToString()), pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            methodology.IsActive = !methodology.IsActive;
            _methodologyService.Update(methodology);
            _methodologyService.Save();
            return RedirectToAction("Index", new { id = Guid.Parse(TempData["Id"].ToString()), pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// حذف متدولوژی
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
            try
            {
                var methodology = _methodologyService.Get(id).MapToEntity();

                #region Remove attachment file
                if (methodology.ImageAttachmentId.HasValue)
                    _attachmentFileService.RemoveAttachment(methodology.ImageAttachmentId.Value);

                #endregion

                _methodologyService.Delete(methodology.Id);
                _methodologyService.Save();

                return Json(new
                {
                    Message = Strings.Methodology_Delete_Successfully,
                    Strings.Success,
                    Type = "success"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Message = Strings.Methodology_Delete_Successfully,
                    Strings.Success,
                    Type = "error"
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}