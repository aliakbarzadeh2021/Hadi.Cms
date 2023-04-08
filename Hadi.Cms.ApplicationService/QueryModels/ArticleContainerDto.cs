using System;
using System.Collections.Generic;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class ArticleContainerDto
    {
        public ArticleContainerDto()
        {
            Articles = new List<AllArticleDto>();
        }
        public List<AllArticleDto> Articles { get; set; }
        public Guid? TagsParentId { get; set; }
        public AllArticleDto SpecialArticle { get; set; }
        public bool HaveSpecialArticle { get; set; }
    }
}
