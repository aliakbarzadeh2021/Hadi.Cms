using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class SettingMapper
    {
        public static ISettingDto MapToDto(this Setting instance)
        {
            return AutoMapper.Mapper.Map<Setting, ISettingDto>(instance);
        }

        public static List<ISettingDto> MapToListDto(this List<Setting> instances)
        {
            return AutoMapper.Mapper.Map<List<Setting>, List<ISettingDto>>(instances);
        }

        public static Setting MaptoEntity(this ISettingDto instance)
        {
            return AutoMapper.Mapper.Map<ISettingDto, Setting>(instance);
        }

        public static List<Setting> MaptoEntities(this List<ISettingDto> instances)
        {
            return AutoMapper.Mapper.Map<List<ISettingDto>, List<Setting>>(instances);
        }
    }
}
