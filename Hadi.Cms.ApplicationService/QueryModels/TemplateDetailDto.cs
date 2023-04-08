using Hadi.Cms.Model.Mappings.Interfaces;
using System;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class TemplateDetailDto : ITemplateDetailDto
    {
        public Guid Id { get; set; }
        public Guid TemplateId { get; set; }
        public string Title { get; set; }
        public string Source { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public ITemplateDto TemplateDto { get; set; }
    }
}
