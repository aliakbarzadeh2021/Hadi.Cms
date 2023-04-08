using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IEmployerDto
    {
        Guid CreatedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
        Guid? ModifiedBy { get; set; }
        bool IsDeleted { get; set; }
        Guid Id { get; set; }
        Guid LogoGuid { get; set; }
        string LogoSource { get; set; }
        string Name { get; set; }
        string CEOName { get; set; }
        string Phone { get; set; }
        string Address { get; set; }
        DateTime CreatedDate { get; set; }
        bool IsActive { get; set; }
        ICollection<IProjectDto> ProjectDto { get; set; }
    }
}
