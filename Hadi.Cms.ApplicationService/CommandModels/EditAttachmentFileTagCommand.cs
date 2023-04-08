using System;
using System.Collections.Generic;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    public class EditAttachmentFileTagCommand
    {
        public EditAttachmentFileTagCommand()
        {
            Tags = new List<Guid>();
        }
        public Guid AttachmentId { get; set; }
        public List<Guid> Tags { get; set; }
    }
}
