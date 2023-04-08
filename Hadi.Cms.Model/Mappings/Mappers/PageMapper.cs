using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class PageMapper
    {
        public static IPageDto MapToDto(this Page instance)
        {
            return AutoMapper.Mapper.Map<Page, IPageDto>(instance);
        }

        public static List<IPageDto> MapToListDto(this List<Page> instances)
        {
            return AutoMapper.Mapper.Map<List<Page>, List<IPageDto>>(instances);
        }

        public static Page MaptoEntity(this IPageDto instance)
        {
            return AutoMapper.Mapper.Map<IPageDto, Page>(instance);
        }

        public static List<Page> MaptoEntities(this List<IPageDto> instances)
        {
            return AutoMapper.Mapper.Map<List<IPageDto>, List<Page>>(instances);
        }
    }
}
