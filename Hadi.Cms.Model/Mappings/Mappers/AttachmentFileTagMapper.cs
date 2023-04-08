using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class AttachmentFileTagMapper
    {
        public static List<IAttachmentFileTagDto> MapToListDto(this List<AttachmentFileTag> instance)
        {
            return Mapper.Map<List<IAttachmentFileTagDto>>(instance);
        }

        public static List<AttachmentFileTag> MapToListEntity(this List<IAttachmentFileTagDto> instance)
        {
            return Mapper.Map<List<AttachmentFileTag>>(instance);
        }

        public static IAttachmentFileTagDto MapToDto(this AttachmentFileTag instance)
        {
            return Mapper.Map<IAttachmentFileTagDto>(instance);
        }

        public static AttachmentFileTag MapToEntity(this IAttachmentFileTagDto instance)
        {
            return Mapper.Map<AttachmentFileTag>(instance);
        }
    }
}
