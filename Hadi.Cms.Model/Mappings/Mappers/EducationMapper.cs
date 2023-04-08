using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class EducationMapper
    {
        public static IEducationDto MapToDto(this Education instance)
        {
            return AutoMapper.Mapper.Map<Education, IEducationDto>(instance);
        }

        public static List<IEducationDto> MapToListDto(this List<Education> instances)
        {
            return AutoMapper.Mapper.Map<List<Education>, List<IEducationDto>>(instances);
        }

        public static Education MaptoEntity(this IEducationDto instance)
        {
            return AutoMapper.Mapper.Map<IEducationDto, Education>(instance);
        }

        public static List<Education> MaptoEntities(this List<IEducationDto> instances)
        {
            return AutoMapper.Mapper.Map<List<IEducationDto>, List<Education>>(instances);
        }
    }
}
