using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;


namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class IpRangeMapper
    {
        public static IIpRangeDto MapToDto(this IpRange instance)
        {
            return AutoMapper.Mapper.Map<IpRange, IIpRangeDto>(instance);
        }

        public static List<IIpRangeDto> MapToListDto(this List<IpRange> instances)
        {
            return AutoMapper.Mapper.Map<List<IpRange>, List<IIpRangeDto>>(instances);
        }

        public static IpRange MaptoEntity(this IIpRangeDto instance)
        {
            return AutoMapper.Mapper.Map<IIpRangeDto, IpRange>(instance);
        }

        public static List<IpRange> MaptoEntities(this List<IIpRangeDto> instances)
        {
            return AutoMapper.Mapper.Map<List<IIpRangeDto>, List<IpRange>>(instances);
        }
    }
}
