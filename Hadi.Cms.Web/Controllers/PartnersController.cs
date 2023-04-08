using System.Web.Mvc;
using Hadi.Cms.ApplicationService.Services;

namespace Hadi.Cms.Web.Controllers
{
    /// <summary>
    /// همکاران
    /// </summary>
    public class PartnersController : Controller
    {
        private readonly PartnerService _partnerService;
        private readonly AttachmentFileService _attachmentFileService;

        public PartnersController()
        {
            _partnerService = new PartnerService();
            _attachmentFileService = new AttachmentFileService();
        }

        [ChildActionOnly]
        public ActionResult PartnersPartial()
        {
            var partners = _partnerService.GetList(p => p.IsActive && !p.IsDeleted);

            foreach (var partner in partners)
                partner.AttachmentImageSource =
                    _attachmentFileService.GetAttachmentSourceValue(partner.AttachmentImageGuid);

            return PartialView("_PartnersPartial",partners);
        }
    }
}