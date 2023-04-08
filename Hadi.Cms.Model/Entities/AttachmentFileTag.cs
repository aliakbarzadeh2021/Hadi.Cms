using System;

namespace Hadi.Cms.Model.Entities
{
    public class AttachmentFileTag : BaseModel
    {
        public AttachmentFileTag()
        {

        }
        public Guid AttachmentFileId { get; set; }
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
        public AttachmentFile AttachmentFile { get; set; }
    }
}
