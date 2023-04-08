using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class NewsCategoryMapper
    {
        public static List<INewsCategory> MapToListDto(this List<NewsCategory> instance)
        {
            return Mapper.Map<List<INewsCategory>>(instance);
        }

        public static List<NewsCategory> MapToListEntity(this List<INewsCategory> instance)
        {
            return Mapper.Map<List<NewsCategory>>(instance);
        }

        public static INewsCategory MapToDto(this NewsCategory instance)
        {
            return Mapper.Map<INewsCategory>(instance);
        }

        public static NewsCategory MapToEntity(this INewsCategory instance)
        {
            return Mapper.Map<NewsCategory>(instance);
        }
    }
}
