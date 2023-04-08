using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IAttachmentFileDto
    {
        Guid Id { get; set; }
        string Title { get; set; }
        string Name { get; set; }
        string Extension { get; set; }
        int Size { get; set; }
        string MimeType { get; set; }
        bool IsPhysicalStorage { get; set; }
        bool IsBinaryStorage { get; set; }
        byte[] Binary { get; set; }
        string PhysicalPath { get; set; }
        int? ImageWidth { get; set; }
        int? ImageHeight { get; set; }
        string Description { get; set; }
        DateTime LastModified { get; set; }
        Guid CreatedBy { get; set; }
        Guid? PosterImageGuid { get; set; }
        ICollection<IMailAttachmentDto> MailAttachmentsDto { get; set; }
        ICollection<IArticleDto> ArticlesDto { get; set; }

        object Source { get; set; }
        string ShowImage { get; set; }
        string CreatedByUserName { get; set; }
        ICollection<IAttachmentFileTagDto> AttachmentFileTagDto { get; set; }
        ICollection<IProjectAttachmentFileDto> ProjectAttachmentFilesDto { get; set; }
        ICollection<ICourseAttachmentFileDto> CourseAttachmentFileDto { get; set; }
    }
}
