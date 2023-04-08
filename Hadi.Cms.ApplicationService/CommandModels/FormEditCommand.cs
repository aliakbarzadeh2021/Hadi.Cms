using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    public class FormEditCommand
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// شناسه زبان فرم
        /// </summary>
        public Guid LanguageId { get; set; }
        /// <summary>
        /// نام فرم
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Strings))]
        public string Name { get; set; }
        /// <summary>
        /// عنوان
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Strings))]
        public string Title { get; set; }
        /// <summary>
        /// نام نمایشی فارسی
        /// </summary>
        public string DisplayFaName { get; set; }
        /// <summary>
        /// نام نمایشی لاتین
        /// </summary>
        public string DisplayEnName { get; set; }
        /// <summary>
        /// آدرس بازگشتی
        /// </summary>
        public string RedirectUrl { get; set; }
        /// <summary>
        /// فیلد های فرم
        /// </summary>
        [AllowHtml]
        public string FormDataSource { get; set; }
    }
}
