using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class FooterCategoryMapper
    {
        public static List<IFooterCategoryDto> MapToListDto(this List<FooterCategory> instances)
        {
            return Mapper.Map<List<IFooterCategoryDto>>(instances);
        }

        public static List<FooterCategory> MapToEntities(this List<IFooterCategoryDto> instances)
        {
            return Mapper.Map<List<FooterCategory>>(instances);
        }

        public static IFooterCategoryDto MapToDto(this FooterCategory instance)
        {
            return Mapper.Map<IFooterCategoryDto>(instance);
        }

        public static FooterCategory MapToEntity(this IFooterCategoryDto instance)
        {
            return Mapper.Map<FooterCategory>(instance);
        }
    }
}
