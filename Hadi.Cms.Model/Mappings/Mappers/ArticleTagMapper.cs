using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class ArticleTagMapper
    {
        public static IArticleTagDto MapToDto(this ArticleTag instance)
        {
            return AutoMapper.Mapper.Map<ArticleTag, IArticleTagDto>(instance);
        }

        public static List<IArticleTagDto> MapToListDto(this List<ArticleTag> instances)
        {
            return AutoMapper.Mapper.Map<List<ArticleTag>, List<IArticleTagDto>>(instances);
        }

        public static ArticleTag MaptoEntity(this IArticleTagDto instance)
        {
            return AutoMapper.Mapper.Map<IArticleTagDto, ArticleTag>(instance);
        }

        public static List<ArticleTag> MaptoEntities(this List<IArticleTagDto> instances)
        {
            return AutoMapper.Mapper.Map<List<IArticleTagDto>, List<ArticleTag>>(instances);
        }
    }
}
