using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface INlMessageDto
    {
        Guid Id { get; set; }
        string Title { get; set; }
        string Subject { get; set; }
        string Text { get; set; }
        Guid? AttachmentId { get; set; }
        DateTime CreatedDate { get; set; }
        Guid CreatedBy { get; set; }
        string AttachmentSource { get; set; }
        ICollection<INlMessageEmailDto> NlMessageEmailDto { get; set; }
    }
}
