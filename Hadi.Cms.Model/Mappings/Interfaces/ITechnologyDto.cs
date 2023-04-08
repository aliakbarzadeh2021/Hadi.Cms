using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface ITechnologyDto
    {
        Guid Id { get; set; }
        DateTime CreatedDate { get; set; }
        bool IsActive { get; set; }
        string Name { get; set; }
        Guid? ImageGuid { get; set; }
        string ImageSource { get; set; }
        bool IsTool { get; set; }
        Guid CreatedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
        Guid? ModifiedBy { get; set; }
        bool IsDeleted { get; set; }
        ICollection<IProjectTechnologyDto> ProjectTechnologiesDto { get; set; }
    }
}
