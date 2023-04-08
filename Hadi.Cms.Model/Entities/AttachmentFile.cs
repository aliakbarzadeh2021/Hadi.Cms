using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// فایل ضمیمه
    /// </summary>
    public class AttachmentFile
    {
        public AttachmentFile()
        {
            Id = Guid.NewGuid();
            LastModified = DateTime.Now;
            Articles = new HashSet<Article>();
            AttachmentFileTags = new HashSet<AttachmentFileTag>();
            ProjectAttachmentFiles = new HashSet<ProjectAttachmentFile>();
            AttachmentFileTags = new List<AttachmentFileTag>();
            CourseAttachmentFiles = new HashSet<CourseAttachmentFile>();
        }

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
        public virtual ICollection<MailAttachment> MailAttachments { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
        public ICollection<AttachmentFileTag> AttachmentFileTags { get; set; }
        public ICollection<ProjectAttachmentFile> ProjectAttachmentFiles { get; set; }
        public ICollection<CourseAttachmentFile> CourseAttachmentFiles { get; set; }
    }
}
