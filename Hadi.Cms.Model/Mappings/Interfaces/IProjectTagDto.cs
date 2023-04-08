using System;
using Hadi.Cms.Model.Entities;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IProjectTagDto
    {
        Guid Id { get; set; }
        Guid ProjectId { get; set; }
        Guid TagId { get; set; }
        DateTime CreatedDate { get; set; }
        Guid CreatedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
        Guid? ModifiedBy { get; set; }
        bool IsActive { get; set; }
        bool IsDeleted { get; set; }
        ITagDto TagDto { get; set; }
        IProjectDto ProjectDto { get; set; }
    }
}
