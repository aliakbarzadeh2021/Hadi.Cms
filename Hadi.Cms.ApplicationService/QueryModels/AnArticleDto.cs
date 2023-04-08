using System;
using System.Collections.Generic;
using System.Windows.Documents;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class AnArticleDto
    {
        public AnArticleDto()
        {
            Tags = new List<ITagDto>();
        }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string ArticleImageSource { get; set; }
        public string ShamsiStringDate { get; set; }
        public string AuthorName { get; set; }
        public string AuthorImageSource { get; set; }
        public string Source { get; set; }
        public string ImageCaption { get; set; }
        public List<ITagDto> Tags { get; set; }
        public string TelegramAddress { get; set; }
        public string InstagramAddress { get; set; }
        public string LinkedInAddress { get; set; }
    }
}
