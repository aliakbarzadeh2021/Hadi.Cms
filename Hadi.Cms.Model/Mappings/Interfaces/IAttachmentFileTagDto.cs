using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
     public interface IAttachmentFileTagDto
    {
         Guid Id { get; set; }
         Guid AttachmentFileId { get; set; }
         Guid TagId { get; set; }
         ITagDto TagDto { get; set; }
         IAttachmentFileDto AttachmentFileDto { get; set; }
    }
}
