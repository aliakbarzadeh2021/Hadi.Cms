using System.Collections.Generic;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// دسته بندی
    /// </summary>
    public class NewsCategory : BaseModel
    {
        public NewsCategory()
        {
            NewsNewsCategories = new HashSet<NewsNewsCategory>();
        }
        /// <summary>
        /// عنوان دسته بندی
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// عنوان دسته بندی لاتین
        /// </summary>
        public string EnTitle { get; set; }
        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public int OrderId { get; set; }
        public ICollection<NewsNewsCategory> NewsNewsCategories { get; set; }
    }
}
