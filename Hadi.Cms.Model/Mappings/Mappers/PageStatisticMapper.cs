using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class PageStatisticMapper
    {
        public static IPageStatisticDto MapToDto(this PageStatistic instance)
        {
            return AutoMapper.Mapper.Map<PageStatistic, IPageStatisticDto>(instance);
        }

        public static List<IPageStatisticDto> MapToListDto(this List<PageStatistic> instances)
        {
            return AutoMapper.Mapper.Map<List<PageStatistic>, List<IPageStatisticDto>>(instances);
        }

        public static PageStatistic MaptoEntity(this IPageStatisticDto instance)
        {
            return AutoMapper.Mapper.Map<IPageStatisticDto, PageStatistic>(instance);
        }

        public static List<PageStatistic> MaptoEntities(this List<IPageStatisticDto> instances)
        {
            return AutoMapper.Mapper.Map<List<IPageStatisticDto>, List<PageStatistic>>(instances);
        }
    }
}
