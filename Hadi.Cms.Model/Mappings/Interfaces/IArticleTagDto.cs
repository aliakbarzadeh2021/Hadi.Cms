using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IArticleTagDto
    {
        Guid Id { get; set; }
        Guid ArticleId { get; set; }
        Guid TagId { get; set; }

        IArticleDto ArticleDto { get; set; }
        ITagDto TagDto { get; set; }
    }
}
