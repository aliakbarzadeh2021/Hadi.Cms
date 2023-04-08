using System;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// دسته بندی اخبار
    /// </summary>
    public class NewsNewsCategory : BaseModel
    {
        public NewsNewsCategory()
        {
            
        }
        /// <summary>
        /// آیدی خبر
        /// </summary>
        public Guid NewsId { get; set; }
        /// <summary>
        /// آیدی دسته بندی
        /// </summary>
        public Guid NewsCategoryId { get; set; }
        public News News { get; set; }
        public NewsCategory NewsCategory { get; set; }
    }
}
