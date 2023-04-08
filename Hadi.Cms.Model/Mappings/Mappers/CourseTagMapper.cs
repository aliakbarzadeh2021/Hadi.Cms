using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class CourseTagMapper
    {
        public static List<ICourseTagDto> MapToListDto(this List<CourseTag> instances)
        {
            return Mapper.Map<List<ICourseTagDto>>(instances);
        }

        public static List<CourseTag> MapToEntities(this List<ICourseTagDto> instances)
        {
            return Mapper.Map<List<CourseTag>>(instances);
        }

        public static ICourseTagDto MapToDto(this CourseTag instance)
        {
            return Mapper.Map<ICourseTagDto>(instance);
        }

        public static CourseTag MapToList(this ICourseTagDto instance)
        {
            return Mapper.Map<CourseTag>(instance);
        }
    }
}
