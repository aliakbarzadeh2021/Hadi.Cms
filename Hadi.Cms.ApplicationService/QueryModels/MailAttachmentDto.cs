using Hadi.Cms.Model.Mappings.Interfaces;
using System;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class MailAttachmentDto : IMailAttachmentDto
    {
        public Guid Id { get; set; }
        public Guid MailId { get; set; }
        public Guid AttachmentId { get; set; }
    }
}
