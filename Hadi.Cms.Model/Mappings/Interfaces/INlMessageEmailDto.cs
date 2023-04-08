using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface INlMessageEmailDto
    {
        Guid Id { get; set; }
        Guid NlEmailId { get; set; }
        Guid NlMessageId { get; set; }
        bool Published { get; set; }
        INlEmailDto NlEmailDto { get; set; }
        INlMessageDto NlMessageDto { get; set; }
    }
}
