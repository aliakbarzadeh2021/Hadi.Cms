using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// فرم
    /// </summary>
    public class Form : BaseModel
    {
        public Form()
        {
            FormFields = new HashSet<FormField>();
        }

        /// <summary>
        /// شناسه زبان فرم
        /// </summary>
        public Guid LanguageId { get; set; }

        /// <summary>
        /// نام فرم
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// عنوان
        /// </summary>
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

        public ICollection<FormField> FormFields { get; set; }
    }
}
