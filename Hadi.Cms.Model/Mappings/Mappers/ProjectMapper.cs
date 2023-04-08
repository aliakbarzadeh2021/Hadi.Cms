using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class ProjectMapper
    {
        public static List<Project> MapToEntities(this List<IProjectDto> instances)
        {
            return Mapper.Map<List<Project>>(instances);
        }

        public static List<IProjectDto> MapToListDto(this List<Project> instances)
        {
            return Mapper.Map<List<IProjectDto>>(instances);
        }

        public static Project MapToEntity(this IProjectDto instance)
        {
            return Mapper.Map<Project>(instance);
        }

        public static IProjectDto MapToDto(this Project instance)
        {
            return Mapper.Map<IProjectDto>(instance);
        }
    }
}
