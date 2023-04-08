using System;
using System.Web;
using System.Web.Mvc;
using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Infrastructure.Helpers;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Web.Utilities;

namespace Hadi.Cms.Web.Controllers
{
    /// <summary>
    /// رزومه
    /// </summary>
    public class ResumesController : Controller
    {
        private readonly ResumeService _resumeService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly EventLoger _eventLoger;
        public ResumesController()
        {
            _resumeService = new ResumeService();
            _attachmentFileService = new AttachmentFileService();
            _eventLoger = new EventLoger();
        }

        /// <summary>
        /// ثبت رزومه جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="resumeFile"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(ResumeCreateCommand command, HttpPostedFileBase resumeFile)
        {
            try
            {
                #region Insert resume attachment file to attachment file table

                if (resumeFile != null && resumeFile.ContentLength > 0)
                {
                    byte[] fileData;
                    var fileSize = resumeFile.ContentLength;
                    
                    using (var binaryReader = new System.IO.BinaryReader(resumeFile.InputStream))
                        fileData = binaryReader.ReadBytes(resumeFile.ContentLength);

                    var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(resumeFile.FileName));

                    var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(resumeFile.FileName),
                        System.IO.Path.GetExtension(resumeFile.FileName), fileSize, mimeType,
                        0, 0, "ResumeFile - " + Guid.NewGuid(), "", fileData, DateTime.Now);

                    command.AttachmentFileId = attachmentId;
                }

                #endregion

                var newResumeId = _resumeService.CreateNewResume(command , IpProvider.GetIpValue());
                _eventLoger.LogEvent(EventType.Information, Guid.Empty, "", "ResumesController", "Create", "Success Create Resume", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
                return Json(new
                {
                    Id = newResumeId,
                    Message = "عملیات آپلود رزومه با موفقیت انجام شد",
                    Status = "Success"
                },JsonRequestBehavior.AllowGet);
            }
            
            catch
            {
                return Json(new
                {
                    Message = "متاسفانه خطای سیستمی رخ داده است",
                    Status = "Failed"
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}