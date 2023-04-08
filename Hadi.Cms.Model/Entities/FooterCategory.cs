using System.Collections.Generic;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// دسته بندی های فوتر
    /// </summary>
    public class FooterCategory : BaseModel
    {
        public FooterCategory()
        {
            FooterCategoryLinks = new HashSet<FooterCategoryLink>();
        }

        /// <summary>
        /// عنوان
        /// </summary>
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
        /// <summary>
        /// ترتیب
        /// </summary>
        public int OrderId { get; set; }

        public int? EntityHaveReviewCount { get; set; }
        public ICollection<FooterCategoryLink> FooterCategoryLinks { get; set; }
    }
}
