using System;
using System.Collections.Generic;
using Hadi.Cms.Model.Entities;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IDepartmentDto
    {
        Guid Id { get; set; }
        string Title { get; set; }
        string ContactUsLink { get; set; }
        string Slogan { get; set; }
        string Color { get; set; }
        Guid? AttachmentImageId { get; set; }
        string Source { get; set; }
        DateTime CreatedDate { get; set; }
        Guid CreatedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
        Guid? ModifiedBy { get; set; }
        bool IsActive { get; set; }
        bool IsDeleted { get; set; }
        Guid? DepartmentBackgroundHeaderAttachmentImageId { get; set; }
        Guid? StructureAttachmentImageId { get; set; }
        Guid? JoinOurTeamImageAttachmentId { get; set; }
        string DepartmentBackgroundHeaderAttachmentImageSource { get; set; }
        string StructureAttachmentImageSource { get; set; }
        string JoinOurTeamImageAttachmentSource { get; set; }
        string JoinOurTeamDescription { get; set; }
        bool RecruitmentButtonIsActive { get; set; }
        bool InternshipButtonIsActive { get; set; }
        ICollection<IDepartmentServiceDto> DepartmentServiceDto { get; set; }
        ICollection<IDepartmentSelectionReasonDto> DepartmentSelectionReasonDto { get; set; }
        ICollection<IEmployeeDto> EmployeeDto { get; set; }
        ICollection<IMethodologyDto> MethodologyDto { get; set; }
    }
}
