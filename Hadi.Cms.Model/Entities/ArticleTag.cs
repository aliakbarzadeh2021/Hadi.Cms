using System;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// برچسب های مقاله
    /// </summary>
    public class ArticleTag : BaseModel
    {
        public Guid ArticleId { get; set; }
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
        public Article Article { get; set; }
    }
}
