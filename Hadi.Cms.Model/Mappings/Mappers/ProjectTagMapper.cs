using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class ProjectTagMapper
    {
        public static List<IProjectTagDto> MapToListDto(this List<ProjectTag> instances)
        {
            return Mapper.Map<List<IProjectTagDto>>(instances);
        }

        public static List<ProjectTag> MapToEntities(this List<IProjectTagDto> instances)
        {
            return Mapper.Map<List<ProjectTag>>(instances);
        }

        public static IProjectTagDto MapToDto(this ProjectTag instance)
        {
            return Mapper.Map<IProjectTagDto>(instance);
        }

        public static ProjectTag MapToEntity(this IProjectTagDto instance)
        {
            return Mapper.Map<ProjectTag>(instance);
        }
    }
}
