using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IProjectAttachmentFileDto
    {
        DateTime CreatedDate { get; set; }
        Guid CreatedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
        Guid? ModifiedBy { get; set; }
        bool IsActive { get; set; }
        bool IsDeleted { get; set; }
        Guid Id { get; set; }
        Guid ProjectId { get; set; }
        Guid AttachmentFileId { get; set; }
        IProjectDto ProjectDto { get; set; }
        IAttachmentFileDto AttachmentFileDto { get; set; }
    }
}
