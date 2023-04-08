using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// نویسنده
    /// </summary>
    public class Author : BaseModel
    {
        public Author()
        {
            Articles = new HashSet<Article>();
        }

        /// <summary>
        /// نام کامل
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// شناسه تصویر نویسنده
        /// </summary>
        public Guid? AuthorImageGuid { get; set; }
        /// <summary>
        /// آدرس اینستاگرام
        /// </summary>
        public string InstagramAddress { get; set; }
        /// <summary>
        /// آدرس تلگرام
        /// </summary>
        public string TelegramAddress { get; set; }
        /// <summary>
        /// آدرس لینکدین
        /// </summary>
        public string LinkedInAddress { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
