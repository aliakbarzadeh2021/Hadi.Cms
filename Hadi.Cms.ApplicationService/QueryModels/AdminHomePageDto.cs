using System.Collections.Generic;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class AdminHomePageDto
    {
        public AdminHomePageDto()
        {
            Articles = new List<IArticleDto>();
            Services = new List<IServiceDto>();
            Tags = new List<ITagDto>();
        }
        public List<IArticleDto> Articles { get; set; }
        public List<IServiceDto> Services { get; set; }
        public List<ITagDto> Tags { get; set; }
    }
}
