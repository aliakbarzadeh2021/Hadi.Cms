using Hadi.Cms.ApplicationService.Services;
using System;
using System.Web.Mvc;

namespace Hadi.Cms.Web.Controllers
{
    public class AttachmentsController : Controller
    {
        private AttachmentFileService _attachmentFileService;

        public AttachmentsController()
        {
            _attachmentFileService = new AttachmentFileService();
        }

        public ActionResult GetAttachment(Guid Id)
        {
            var attachment = _attachmentFileService.Get(a => a.Id == Id);
            return File(attachment.Binary, attachment.MimeType);
        }
    }
}