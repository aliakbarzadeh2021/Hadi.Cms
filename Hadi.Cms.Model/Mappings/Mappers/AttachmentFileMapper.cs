using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class AttachmentFileMapper
    {
        public static IAttachmentFileDto MapToDto(this AttachmentFile instance)
        {
            return AutoMapper.Mapper.Map<AttachmentFile, IAttachmentFileDto>(instance);
        }

        public static List<IAttachmentFileDto> MapToListDto(this List<AttachmentFile> instances)
        {
            return AutoMapper.Mapper.Map<List<AttachmentFile>, List<IAttachmentFileDto>>(instances);
        }

        public static AttachmentFile MaptoEntity(this IAttachmentFileDto instance)
        {
            return AutoMapper.Mapper.Map<IAttachmentFileDto, AttachmentFile>(instance);
        }

        public static List<AttachmentFile> MaptoEntities(this List<IAttachmentFileDto> instances)
        {
            return AutoMapper.Mapper.Map<List<IAttachmentFileDto>, List<AttachmentFile>>(instances);
        }
    }
}
