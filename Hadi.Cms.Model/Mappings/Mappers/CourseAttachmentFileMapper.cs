using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class CourseAttachmentFileMapper
    {
        public static List<ICourseAttachmentFileDto> MapToListDto(this List<CourseAttachmentFile> instances)
        {
            return Mapper.Map<List<ICourseAttachmentFileDto>>(instances);
        }

        public static List<CourseAttachmentFile> MapToEntities(this List<ICourseAttachmentFileDto> instances)
        {
            return Mapper.Map<List<CourseAttachmentFile>>(instances);
        }

        public static ICourseAttachmentFileDto MapToDto(this CourseAttachmentFile instance)
        {
            return Mapper.Map<ICourseAttachmentFileDto>(instance);
        }

        public static CourseAttachmentFile MapToEntity(this ICourseAttachmentFileDto instance)
        {
            return Mapper.Map<CourseAttachmentFile>(instance);
        }
    }
}
