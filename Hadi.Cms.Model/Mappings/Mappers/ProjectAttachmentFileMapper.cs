using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class ProjectAttachmentFileMapper
    {
        public static List<IProjectAttachmentFileDto> MapToListDto(this List<ProjectAttachmentFile> instances)
        {
            return Mapper.Map<List<IProjectAttachmentFileDto>>(instances);
        }

        public static List<ProjectAttachmentFile> MapToEntities(this List<IProjectAttachmentFileDto> instances)
        {
            return Mapper.Map<List<ProjectAttachmentFile>>(instances);
        }

        public static IProjectAttachmentFileDto MapToDto(this ProjectAttachmentFile instance)
        {
            return Mapper.Map<IProjectAttachmentFileDto>(instance);
        }

        public static ProjectAttachmentFile MapToEntity(this IProjectAttachmentFileDto instance)
        {
            return Mapper.Map<ProjectAttachmentFile>(instance);
        }
    }
}
