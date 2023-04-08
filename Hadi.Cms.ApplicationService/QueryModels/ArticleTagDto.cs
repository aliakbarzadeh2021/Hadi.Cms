using Hadi.Cms.Model.Mappings.Interfaces;
using System;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class ArticleTagDto : IArticleTagDto
    {
        public Guid Id { get; set; }
        public Guid ArticleId { get; set; }
        public Guid TagId { get; set; }
        public IArticleDto ArticleDto { get; set; }
        public ITagDto TagDto { get; set; }
    }
}
