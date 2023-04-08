using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface ISocialNetwork
    {
        Guid Id { get; set; }
        string Title { get; set; }
        string Link { get; set; }
        string Source { get; set; }
        Guid? ImageGuid { get; set; }
        string ImageSource { get; set; }
        DateTime CreatedDate { get; set; }
        Guid CreatedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
        Guid? ModifiedBy { get; set; }
        bool IsActive { get; set; }
        bool IsDeleted { get; set; }
    }
}
