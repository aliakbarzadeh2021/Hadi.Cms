using Hadi.Cms.Model.Mappings.Interfaces;
using System;
using System.Collections.Generic;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class TemplateDto : ITemplateDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Source { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public ICollection<ITemplateDetailDto> TemplateDetailsDto { get; set; }
    }
}
