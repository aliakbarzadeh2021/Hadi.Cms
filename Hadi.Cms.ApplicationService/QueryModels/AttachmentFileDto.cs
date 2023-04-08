using Hadi.Cms.Model.Mappings.Interfaces;
using System;
using System.Collections.Generic;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class AttachmentFileDto : IAttachmentFileDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public int Size { get; set; }
        public string MimeType { get; set; }
        public bool IsPhysicalStorage { get; set; }
        public bool IsBinaryStorage { get; set; }
        public byte[] Binary { get; set; }
        public string PhysicalPath { get; set; }
        public int? ImageWidth { get; set; }
        public int? ImageHeight { get; set; }
        public string Description { get; set; }
        public DateTime LastModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? PosterImageGuid { get; set; }
        public ICollection<IMailAttachmentDto> MailAttachmentsDto { get; set; }
        public ICollection<IArticleDto> ArticlesDto { get; set; }

        public object Source { get; set; }
        public string ShowImage { get; set; }
        public string CreatedByUserName { get; set; }
        public ICollection<IAttachmentFileTagDto> AttachmentFileTagDto { get; set; }
        public ICollection<IProjectAttachmentFileDto> ProjectAttachmentFilesDto { get; set; }
        public ICollection<ICourseAttachmentFileDto> CourseAttachmentFileDto { get; set; }
    }
}
