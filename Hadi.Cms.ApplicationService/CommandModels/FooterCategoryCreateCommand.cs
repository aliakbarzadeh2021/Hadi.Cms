using System.ComponentModel.DataAnnotations;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ثبت دسته بندی فوتر
    /// </summary>
    public class FooterCategoryCreateCommand
    {
        /// <summary>
        /// عنوان
        /// </summary>
        [Required(AllowEmptyStrings = false , ErrorMessageResourceType = typeof(Strings),ErrorMessageResourceName = "Required")]
        public string Title { get; set; }
        /// <summary>
        /// لینک
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// نمایش آماری
        /// </summary>
        public bool StatisticalRepresentation { get; set; }
        /// <summary>
        /// تعداد نمایشی
        /// </summary>
        public int NumberOfShows { get; set; }

        public int? EntityHaveReviewCount { get; set; }
        /// <summary>
        /// ترتیب
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// وضعیت فعال بودن
        /// </summary>
        public bool IsActive { get; set; }
    }
}
