using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IDepartmentSelectionReasonDto
    {
        Guid Id { get; set; }
        Guid DepartmentId { get; set; }
        DateTime CreatedDate { get; set; }
        Guid CreatedBy { get; set; }
        bool IsActive { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        Guid? AttachmentImageId { get; set; }
        string ImageSource { get; set; }
        IDepartmentDto DepartmentDto { get; set; }
    }
}
