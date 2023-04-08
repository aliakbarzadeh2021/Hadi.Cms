using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Language.Resources;
using Hadi.Cms.Web.Utilities;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// مدیریت رزومه ها
    /// </summary>
    public class ResumesController : Controller
    {
        private readonly AttachmentFileService _attachmentFileService;
        private readonly ResumeService _resumeService;

        public ResumesController()
        {
            _attachmentFileService = new AttachmentFileService();
            _resumeService = new ResumeService();
        }

        /// <summary>
        /// دریافت لیست رزومه ها
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(Guid id, int pageNumber = 1)
        {
            var pageSize = 5;
            var resumes = _resumeService.GetList(r => r.IsActive && !r.IsDeleted && r.CareerOpportunityId == id, o => o.OrderByDescending(r => r.CreatedDate));
            foreach (var resume in resumes)
            {
                var attachmentFile = _attachmentFileService.GetById(resume.AttachmentFileId);
                if (attachmentFile != null)
                {
                    resume.AttachmentFileName = attachmentFile.Name;
                    resume.AttachmentFileType = attachmentFile.MimeType;
                    resume.StorageType = attachmentFile.IsBinaryStorage ? "Binary" : "PhysicalStorage";

                    if (attachmentFile.IsBinaryStorage)
                        resume.FileSource = "/Attachments/GetAttachment/" + resume.AttachmentFileId;

                    else
                        resume.FileSource = attachmentFile.PhysicalPath;
                }
            }

            var pagedList = resumes.ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;

            return View(pagedList);
        }

        /// <summary>
        /// حذف رزومه
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
            try
            {
                var resume = _resumeService.Get(id);

                #region Remove resume from attachment file
                if (resume.AttachmentFileId.HasValue)
                    _attachmentFileService.RemoveAttachment(resume.AttachmentFileId.Value);
                #endregion

                _resumeService.Delete(resume.Id);
                _resumeService.Save();

                return Json(new
                {
                    Message = Strings.Delete_Resume_Successfully,
                    Success = Strings.Success
                });
            }

            catch
            {
                return Json(new
                {
                    Message = Strings.Global_SystemError,
                    Success = Strings.Error
                });
            }
        }
    }
}