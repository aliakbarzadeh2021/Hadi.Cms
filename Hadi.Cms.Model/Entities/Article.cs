using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Entities
{
    public class Article : BaseModel
    {
        public Article()
        {
            ArticleTags = new HashSet<ArticleTag>();
        }

        public Guid? AuthorId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Source { get; set; }
        public string ImageCaption { get; set; }
        public int ReviewCount { get; set; }
        public Guid? AttachmentImageId { get; set; }
        public Guid LanguageId { get; set; }
        public bool IsSpecial { get; set; }
        public DateTime? IsSpecialCreatedDate { get; set; }

        public ICollection<ArticleTag> ArticleTags { get; set; }
        public Author Author { get; set; }
    }
}
