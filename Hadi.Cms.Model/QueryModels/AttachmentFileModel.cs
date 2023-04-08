using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.QueryModels
{
    public class AttachmentFileModel
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
        public string Source { get; set; }
        public string CreatedByUserName { get; set; }
        public List<Guid> TagsId { get; set; }
        public Guid? PosterImageGuid { get; set; }
    }
}
