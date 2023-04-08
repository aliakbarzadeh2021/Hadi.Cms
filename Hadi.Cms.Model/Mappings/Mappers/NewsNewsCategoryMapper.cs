using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class NewsNewsCategoryMapper
    {
        public static List<NewsNewsCategory> MapToListEntity(this List<INewsNewsCategory> instance)
        {
            return Mapper.Map<List<NewsNewsCategory>>(instance);
        }

        public static List<INewsNewsCategory> MapToListDto(this List<NewsNewsCategory> instance)
        {
            return Mapper.Map<List<INewsNewsCategory>>(instance);
        }

        public static NewsNewsCategory MapToEntity(this INewsNewsCategory instance)
        {
            return Mapper.Map<NewsNewsCategory>(instance);
        }

        public static INewsNewsCategory MapTDto(this NewsNewsCategory instance)
        {
            return Mapper.Map<INewsNewsCategory>(instance);
        }
    }
}
