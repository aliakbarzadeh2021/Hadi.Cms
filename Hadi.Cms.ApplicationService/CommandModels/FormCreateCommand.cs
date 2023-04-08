using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ثبت فرم
    /// </summary>
    public class FormCreateCommand
    {
        public FormCreateCommand()
        {
            FormFields = new List<FormFieldCommand>();
        }
        /// <summary>
        /// شناسه زبان فرم
        /// </summary>
        public Guid LanguageId { get; set; }
        /// <summary>
        /// نام فرم
        /// </summary>
        [Required(AllowEmptyStrings = false , ErrorMessageResourceName = "Required" , ErrorMessageResourceType = typeof(Strings))]
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

        public Guid Id { get; set; }

        public List<FormFieldCommand> FormFields { get; set; }

    }

    public class FormFieldCommand
    {
        /// <summary>
        /// لیبل اینپوت
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// نام
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// نوع فیلد
        /// </summary>
        public string Type { get; set; }
    }
}
