using System;
using System.Linq;
using System.Web.Mvc;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Model.Enums;

namespace Hadi.Cms.Web.Controllers
{
    /// <summary>
    /// فرصت های شغلی
    /// </summary>
    public class CareerOpportunitiesController : Controller
    {
        private static int _careerOpportunityTypeValue;
        private readonly CareerOpportunityService _careerOpportunityService;
        private readonly AttachmentFileService _attachmentFileService;

        public CareerOpportunitiesController()
        {
            _careerOpportunityService = new CareerOpportunityService();
            _attachmentFileService = new AttachmentFileService();
        }

        /// <summary>
        /// ست کردن مقدار نوع شمارشی فرصت های شغلی - استخدامی یا کارآموزی
        /// </summary>
        /// <param name="typeValue"></param>
        /// <returns></returns>

        public ActionResult SetCareerOpportunityTypeValue(int typeValue)
        {
            _careerOpportunityTypeValue = typeValue;
            return Json(new
            {
                Message = "Done"
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// گت کردن مقدار نوع شمارشی فرصت های شغلی - استخدامی یا کارآموزی
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public ActionResult GetCareerOpportunityTypeValue()
        {
            return Json(new
            {
                CareerOpportunityTypeValue = _careerOpportunityTypeValue
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// دریافت لیست فرصت های شغلی
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCareerOpportunities()
        {
            var careerOpportunities = _careerOpportunityService.GetList(c => c.IsActive && !c.IsDeleted);

            foreach (var item in careerOpportunities)
                item.CareerOpportunityImageSource =
                    _attachmentFileService.GetAttachmentSourceValue(item.CareerOpportunityImageGuid);

             
            var dto = careerOpportunities.Select(c => new
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                CareerOpportunityImageSource = _attachmentFileService.GetAttachmentSourceValue(c.CareerOpportunityImageGuid),
                CareerOpportunityType = c.CareerOpportunityType
            }).ToList();

            return Json(new
            {
                Employment = dto.Where(o => o.CareerOpportunityType == CareerOpportunityType.Employment).ToList(),
                Internship = dto.Where(o => o.CareerOpportunityType == CareerOpportunityType.Internship).ToList()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}