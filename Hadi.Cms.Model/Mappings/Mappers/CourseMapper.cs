using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class CourseMapper
    {
        public static List<ICourseDto> MapToListDto(this List<Course> instances)
        {
            return Mapper.Map<List<ICourseDto>>(instances);
        }

        public static List<Course> MapToEntities(this List<ICourseDto> instances)
        {
            return Mapper.Map<List<Course>>(instances);
        }

        public static ICourseDto MapToDto(this Course instance)
        {
            return Mapper.Map<ICourseDto>(instance);
        }

        public static Course MapToEntity(this ICourseDto instance)
        {
            return Mapper.Map<Course>(instance);
        }
    }
}
