using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface INlEmailDto
    {
        Guid Id { get; set; }
        string Email { get; set; }
        string ShamsiCreatedDate { get; set; }
        DateTime CreatedDate { get; set; }
        ICollection<INlMessageEmailDto> NlMessageEmailDto { get; set; }
    }
}
