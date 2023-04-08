using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface ITemplateDetailDto
    {
        Guid Id { get; set; }
        Guid TemplateId { get; set; }
        string Title { get; set; }
        string Source { get; set; }
        DateTime CreatedDate { get; set; }
        Guid CreatedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
        Guid? ModifiedBy { get; set; }
        bool IsDeleted { get; set; }
        bool IsActive { get; set; }
        ITemplateDto TemplateDto { get; set; }
    }
}
