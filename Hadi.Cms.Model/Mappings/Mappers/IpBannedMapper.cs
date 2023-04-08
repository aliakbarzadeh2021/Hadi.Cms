using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class IpBannedMapper
    {
        public static IIpBannedDto MapToDto(this IpBanned instance)
        {
            return AutoMapper.Mapper.Map<IpBanned, IIpBannedDto>(instance);
        }

        public static List<IIpBannedDto> MapToListDto(this List<IpBanned> instances)
        {
            return AutoMapper.Mapper.Map<List<IpBanned>, List<IIpBannedDto>>(instances);
        }

        public static IpBanned MaptoEntity(this IIpBannedDto instance)
        {
            return AutoMapper.Mapper.Map<IIpBannedDto, IpBanned>(instance);
        }

        public static List<IpBanned> MaptoEntities(this List<IIpBannedDto> instances)
        {
            return AutoMapper.Mapper.Map<List<IIpBannedDto>, List<IpBanned>>(instances);
        }

    }
}
