using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class ConfirmKeyMapper
    {
        public static IConfirmKeyDto MapToDto(this ConfirmKey instance)
        {
            return AutoMapper.Mapper.Map<ConfirmKey, IConfirmKeyDto>(instance);
        }

        public static List<IConfirmKeyDto> MapToListDto(this List<ConfirmKey> instances)
        {
            return AutoMapper.Mapper.Map<List<ConfirmKey>, List<IConfirmKeyDto>>(instances);
        }

        public static ConfirmKey MaptoEntity(this IConfirmKeyDto instance)
        {
            return AutoMapper.Mapper.Map<IConfirmKeyDto, ConfirmKey>(instance);
        }

        public static List<ConfirmKey> MaptoEntities(this List<IConfirmKeyDto> instances)
        {
            return AutoMapper.Mapper.Map<List<IConfirmKeyDto>, List<ConfirmKey>>(instances);
        }
    }
}
