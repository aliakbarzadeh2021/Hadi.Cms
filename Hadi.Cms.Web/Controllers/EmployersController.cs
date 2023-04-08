using System.Linq;
using System.Web.Mvc;
using Hadi.Cms.ApplicationService.Services;

namespace Hadi.Cms.Web.Controllers
{
    /// <summary>
    /// کارفرمایان
    /// </summary>
    public class EmployersController : Controller
    {
        private readonly EmployerService _employerService;
        private readonly AttachmentFileService _attachmentFileService;

        public EmployersController()
        {
            _employerService = new EmployerService();
            _attachmentFileService = new AttachmentFileService();
        }
        
        [ChildActionOnly]
        public ActionResult EmployersPartial()
        {
            var employers = _employerService.GetList(e => e.IsActive && !e.IsDeleted , e => e.OrderByDescending(o => o.CreatedDate),e => e.Projects);

            employers = employers.Where(e => e.ProjectDto.Any()).ToList();

            foreach (var employer in employers)
                employer.LogoSource = _attachmentFileService.GetAttachmentSourceValue(employer.LogoGuid);

            return PartialView("_EmployersPartial",employers);
        }

        /// <summary>
        /// دریافت تعداد کارفرمایان
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCountOfEmployers()
        {
            // get count of employers
            var countOfEmployers = _employerService.Count(e => e.IsActive && !e.IsDeleted && e.Projects.Any());
            return Json(new
            {
                Count = countOfEmployers
            }, JsonRequestBehavior.AllowGet);
        }
    }
}