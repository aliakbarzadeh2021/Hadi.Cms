using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class MailAttachmentMapper
    {
        public static IMailAttachmentDto MapToDto(this MailAttachment instance)
        {
            return AutoMapper.Mapper.Map<MailAttachment, IMailAttachmentDto>(instance);
        }

        public static List<IMailAttachmentDto> MapToListDto(this List<MailAttachment> instances)
        {
            return AutoMapper.Mapper.Map<List<MailAttachment>, List<IMailAttachmentDto>>(instances);
        }

        public static MailAttachment MaptoEntity(this IMailAttachmentDto instance)
        {
            return AutoMapper.Mapper.Map<IMailAttachmentDto, MailAttachment>(instance);
        }

        public static List<MailAttachment> MaptoEntities(this List<IMailAttachmentDto> instances)
        {
            return AutoMapper.Mapper.Map<List<IMailAttachmentDto>, List<MailAttachment>>(instances);
        }
    }
}
