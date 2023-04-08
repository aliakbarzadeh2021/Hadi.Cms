using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Hadi.Cms.Language.Resources;
using Hadi.Cms.Model.Enums;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ویرایش فرصت شغلی
    /// </summary>
    public class CareerOpportunityEditCommand
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// عنوان فرصت شغلی
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string Title { get; set; }

        public CareerOpportunityType CareerOpportunityType { get; set; }
        /// <summary>
        /// توضیحات فرصت شغلی
        /// </summary>
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        /// <summary>
        /// شناسه تصویر فرصت شغلی
        /// </summary>
        public Guid? CareerOpportunityImageGuid { get; set; }
        /// <summary>
        /// منبع تصویر فرصت شغلی
        /// </summary>
        public string CareerOpportunityImageSource { get; set; }
        /// <summary>
        /// وضعیت فعال بودن
        /// </summary>
        public bool IsActive { get; set; }
    }
}
