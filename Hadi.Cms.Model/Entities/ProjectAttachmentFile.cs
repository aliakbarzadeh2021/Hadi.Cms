using System;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// فایل های پروژه
    /// </summary>
    public class ProjectAttachmentFile : BaseModel
    {
        public ProjectAttachmentFile() { }
        public Guid ProjectId { get; set; }
        public Guid AttachmentFileId { get; set; }
        public Project Project { get; set; }
        public AttachmentFile AttachmentFile { get; set; }
    }
}
