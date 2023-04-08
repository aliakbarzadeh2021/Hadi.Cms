using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class ProjectTechnologyDto
    {
        public static List<IProjectTechnologyDto> MapToListDto(this List<ProjectTechnology> instances)
        {
            return Mapper.Map<List<IProjectTechnologyDto>>(instances);
        }

        public static List<ProjectTechnology> MapToEntities(this List<IProjectTechnologyDto> instances)
        {
            return Mapper.Map<List<ProjectTechnology>>(instances);
        }

        public static IProjectTechnologyDto MapToDto(this ProjectTechnology instance)
        {
            return Mapper.Map<IProjectTechnologyDto>(instance);
        }

        public static ProjectTechnology MapToEntity(this IProjectTechnologyDto instance)
        {
            return Mapper.Map<ProjectTechnology>(instance);
        }
    }
}
