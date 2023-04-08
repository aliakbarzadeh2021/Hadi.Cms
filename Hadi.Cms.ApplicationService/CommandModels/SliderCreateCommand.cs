using System.ComponentModel.DataAnnotations;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ثبت اسلایدر
    /// </summary>
    public class SliderCreateCommand
    {
        /// <summary>
        /// عنوان
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings),ErrorMessageResourceName = "Required")]
        public string Title { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        /// <summary>
        /// وضعیت فعال بودن
        /// </summary>
        public bool IsActive { get; set; }
    }
}
