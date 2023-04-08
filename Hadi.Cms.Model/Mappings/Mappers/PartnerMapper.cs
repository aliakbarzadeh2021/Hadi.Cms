using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class PartnerMapper
    {
        public static List<IPartnerDto> MapToListDto(this List<Partner> instances)
        {
            return Mapper.Map<List<IPartnerDto>>(instances);
        }

        public static List<Partner> MapToEntities(this List<IPartnerDto> instances)
        {
            return Mapper.Map<List<Partner>>(instances);
        }

        public static IPartnerDto MapToDto(this Partner instance)
        {
            return Mapper.Map<IPartnerDto>(instance);
        }

        public static Partner MapToEntity(this IPartnerDto instance)
        {
            return Mapper.Map<Partner>(instance);
        }
    }
}
