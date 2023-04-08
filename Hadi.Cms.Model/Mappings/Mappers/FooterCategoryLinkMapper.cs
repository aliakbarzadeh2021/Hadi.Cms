using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class FooterCategoryLinkMapper
    {
        public static List<IFooterCategoryLinkDto> MapToListDto(this List<FooterCategoryLink> instances)
        {
            return Mapper.Map<List<IFooterCategoryLinkDto>>(instances);
        }

        public static List<FooterCategoryLink> MapToEntities(this List<IFooterCategoryLinkDto> instances)
        {
            return Mapper.Map<List<FooterCategoryLink>>(instances);
        }

        public static IFooterCategoryLinkDto MapToDto(this FooterCategoryLink instance)
        {
            return Mapper.Map<IFooterCategoryLinkDto>(instance);
        }

        public static FooterCategoryLink MapToEntity(this IFooterCategoryLinkDto instance)
        {
            return Mapper.Map<FooterCategoryLink>(instance);
        }
    }
}
