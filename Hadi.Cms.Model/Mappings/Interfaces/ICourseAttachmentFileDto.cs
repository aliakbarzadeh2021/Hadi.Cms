using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface ICourseAttachmentFileDto
    {
        Guid Id { get; set; }
        Guid CourseId { get; set; }
        Guid AttachmentFileId { get; set; }
        ICourseDto CourseDto { get; set; }
        IAttachmentFileDto AttachmentFileDto { get; set; }
    }
}
