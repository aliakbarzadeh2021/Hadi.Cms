using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Hadi.Cms.Language.Resources;
using Hadi.Cms.Model.Enums;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ثبت فرصت شغلی
    /// </summary>
    public class CareerOpportunityCreateCommand
    {
        /// <summary>
        /// عنوان شغل
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string Title { get; set; }
        /// <summary>
        /// نوع شغل
        /// </summary>
        public CareerOpportunityType CareerOpportunityType { get; set; }
        /// <summary>
        /// توضیحات شغل
        /// </summary>
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        /// <summary>
        /// شناسه تصویر
        /// </summary>
        public Guid? CareerOpportunityImageGuid { get; set; }
        /// <summary>
        /// وضعیت فعال بودن
        /// </summary>
        public bool IsActive { get; set; }
    }
}
