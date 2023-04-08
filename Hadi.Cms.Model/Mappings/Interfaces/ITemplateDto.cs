using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface ITemplateDto
    {
        Guid Id { get; set; }
        string Title { get; set; }
        string Source { get; set; }
        DateTime CreatedDate { get; set; }
        Guid CreatedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
        Guid? ModifiedBy { get; set; }
        bool IsDeleted { get; set; }
        bool IsActive { get; set; }
        ICollection<ITemplateDetailDto> TemplateDetailsDto { get; set; }
    }
}
