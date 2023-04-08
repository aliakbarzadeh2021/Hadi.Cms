using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IEmployeeDto
    {
        Guid Id { get; set; }
        Guid DepartmentId { get; set; }
        DateTime CreatedDate { get; set; }
        Guid CreatedBy { get; set; }
        bool IsActive { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        Guid? AttachmentImageId { get; set; }
        string RoleTitle { get; set; }
        int OrderId { get; set; }
        string FullName { get; set; }
        string ImageSource { get; set; }
        ICollection<IDepartmentDto> DepartmentDto { get; set; }
    }
}
