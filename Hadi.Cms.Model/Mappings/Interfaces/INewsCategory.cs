using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface INewsCategory
    {
        Guid Id { get; set; }
        DateTime CreatedDate { get; set; }
        Guid CreatedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
        Guid? ModifiedBy { get; set; }
        bool IsActive { get; set; }
        string Title { get; set; }
        string EnTitle { get; set; }
        int OrderId { get; set; }
        ICollection<INewsNewsCategory> NewsNewsCategories { get; set; }
    }
}
