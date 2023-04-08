using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class NewsMapper
    {
        public static INewsDto MapToDto(this News instance)
        {
            return AutoMapper.Mapper.Map<News, INewsDto>(instance);
        }

        public static List<INewsDto> MapToListDto(this List<News> instances)
        {
            return AutoMapper.Mapper.Map<List<News>, List<INewsDto>>(instances);
        }

        public static News MaptoEntity(this INewsDto instance)
        {
            return AutoMapper.Mapper.Map<INewsDto, News>(instance);
        }

        public static List<News> MaptoEntities(this List<INewsDto> instances)
        {
            return AutoMapper.Mapper.Map<List<INewsDto>, List<News>>(instances);
        }
    }
}
