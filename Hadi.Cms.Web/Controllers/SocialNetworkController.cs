using System.Linq;
using System.Web.Mvc;
using Hadi.Cms.ApplicationService.Services;
using System;
using System.Web.UI.WebControls;

namespace Hadi.Cms.Web.Controllers
{
    public class SocialNetworkController : Controller
    {
        private readonly SocialNetworkService _socialNetworkService;
        private readonly AttachmentFileService _attachmentFileService;

        public SocialNetworkController()
        {
            _socialNetworkService = new SocialNetworkService();
            _attachmentFileService = new AttachmentFileService();
        }

        /// <summary>
        /// پارشیال شبکه های اجتماعی
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult SocialNetworkPartial()
        {
            var socialNetworks = _socialNetworkService.GetList(s => s.IsActive && !s.IsDeleted, o => o.OrderByDescending(s => s.CreatedDate));

            foreach (var socialNetwork in socialNetworks)
                if (socialNetwork.ImageGuid.HasValue)
                    socialNetwork.ImageSource = _attachmentFileService.GetAttachmentSourceValue(socialNetwork.ImageGuid);

            return PartialView("_SocialNetworkPartial", socialNetworks);
        }

        /// <summary>
        /// دریافت لیست شبکه های اجتماعی
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSocialNetworks()
        {
            try
            {
                var socialNetworks = _socialNetworkService.GetList(s => s.IsActive && !s.IsDeleted, o => o.OrderByDescending(s => s.CreatedDate));

                return Json(new
                {
                    Message = "Done",
                    SocialNetworks = socialNetworks.Select(s => new
                    {
                        Id = s.Id,
                        Title = s.Title,
                        Link = s.Link,
                        Source = s.Source ?? "",
                        ImageSource = _attachmentFileService.GetAttachmentSourceValue(s.ImageGuid)
                    }).ToList()
                }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new
                {
                    Message = "Failed"
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}