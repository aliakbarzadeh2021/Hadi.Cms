using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class ArticleMapper
    {
        public static IArticleDto MapToDto(this Article instance)
        {
            return AutoMapper.Mapper.Map<Article, IArticleDto>(instance);
        }

        public static List<IArticleDto> MapToListDto(this List<Article> instances)
        {
            return AutoMapper.Mapper.Map<List<Article>, List<IArticleDto>>(instances);
        }

        public static Article MaptoEntity(this IArticleDto instance)
        {
            return AutoMapper.Mapper.Map<IArticleDto, Article>(instance);
        }

        public static List<Article> MaptoEntities(this List<IArticleDto> instances)
        {
            return AutoMapper.Mapper.Map<List<IArticleDto>, List<Article>>(instances);
        }
    }
}
