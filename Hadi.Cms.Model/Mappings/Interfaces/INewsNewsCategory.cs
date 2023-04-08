using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface INewsNewsCategory
    {
        Guid Id { get; set; }
        DateTime CreatedDate { get; set; }
        Guid CreatedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
        Guid? ModifiedBy { get; set; }
        bool IsActive { get; set; }
        bool IsDeleted { get; set; }
        Guid NewsId { get; set; }
        Guid NewsCategoryId { get; set; }
        INewsCategory NewsCategoryDto { get; set; }
        INewsDto NewsDto { get; set; }
    }
}
