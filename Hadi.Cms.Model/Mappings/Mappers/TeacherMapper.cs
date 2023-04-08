using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class TeacherMapper
    {
        public static List<ITeacherDto> MapToListDto(this List<Teacher> instances)
        {
            return Mapper.Map<List<ITeacherDto>>(instances);
        }

        public static List<Teacher> MapToEntities(this List<ITeacherDto> instances)
        {
            return Mapper.Map<List<Teacher>>(instances);
        }

        public static ITeacherDto MapToDto(this Teacher instances)
        {
            return Mapper.Map<ITeacherDto>(instances);
        }

        public static Teacher MapToEntity(this ITeacherDto instances)
        {
            return Mapper.Map<Teacher>(instances);
        }
    }
}
