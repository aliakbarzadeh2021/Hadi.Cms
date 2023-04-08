using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IBaseModelDto
    {
        Guid Id { get; set; }
        DateTime CreatedDate { get; set; }
        Guid CreatedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
        Guid ModifiedBy { get; set; }
        bool IsDeleted { get; set; }
        bool IsActive { get; set; }
    }
}
