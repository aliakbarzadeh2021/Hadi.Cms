using System.Linq;
using System.Web.Mvc;
using Hadi.Cms.ApplicationService.Services;

namespace Hadi.Cms.Web.Controllers
{
    /// <summary>
    /// اسلایدر
    /// </summary>
    public class SlidersController : Controller
    {
        private readonly SliderService _sliderService;
        private readonly AttachmentFileService _attachmentFileService;

        public SlidersController()
        {
            _sliderService = new SliderService();
            _attachmentFileService = new AttachmentFileService();
        }

        /// <summary>
        /// اسلایدر صفحه ی اصلی سایت
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult HomePageSliderPartial(bool mobileVersion)
        {
            var slider = _sliderService.GetList(s => !s.IsDeleted && s.IsActive, o => o.OrderBy(s => s.CreatedDate), s => s.SliderItems).FirstOrDefault();

            if (slider != null)
            {
                slider.SliderItemDto = slider.SliderItemDto.Where(s => s.IsActive && !s.IsDeleted).ToList();

                foreach (var sliderItem in slider.SliderItemDto)
                    sliderItem.AttachmentImageSource =
                        _attachmentFileService.GetAttachmentSourceValue(sliderItem.AttachmentImageId);
            }

            ViewBag.MobileVersion = mobileVersion;

            return PartialView("_HomePageSliderPartial", slider);
        }
    }
}