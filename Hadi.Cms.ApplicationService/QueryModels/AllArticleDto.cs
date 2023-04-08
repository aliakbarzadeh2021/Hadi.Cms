using System;
using System.Collections.Generic;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class AllArticleDto
    {
        public AllArticleDto()
        {
            Tags = new List<ITagDto>();
        }
        public Guid Id { get; set; }
        public Guid? AuthorId { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string ArticleImageSource { get; set; }
        public string AuthorImageSource { get; set; }
        public Guid? ArticleImageGuid { get; set; }
        public Guid? AuthorImageGuid { get; set; }
        public string ShamsiCreatedDate { get; set; }
        public bool IsSpecial { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<ITagDto> Tags { get; set; }
        public DateTime? IsSpecialCreatedDate { get; set; }
    }
}
