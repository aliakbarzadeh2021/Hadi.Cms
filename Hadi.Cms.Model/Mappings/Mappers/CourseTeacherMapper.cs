using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class CourseTeacherMapper
    {
        public static List<ICourseTeacherDto> MapToListDto(this List<CourseTeacher> instances)
        {
            return Mapper.Map<List<ICourseTeacherDto>>(instances);
        }

        public static List<CourseTeacher> MapToEntities(this List<ICourseTeacherDto> instances)
        {
            return Mapper.Map<List<CourseTeacher>>(instances);
        }

        public static ICourseTeacherDto MapToDto(this CourseTeacher instance)
        {
            return Mapper.Map<ICourseTeacherDto>(instance);
        }

        public static CourseTeacher MapToEntity(this ICourseTeacherDto instance)
        {
            return Mapper.Map<CourseTeacher>(instance);
        }
    }
}
