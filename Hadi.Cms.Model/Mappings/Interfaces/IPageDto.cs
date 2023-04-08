using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IPageDto
    {
        Guid Id { get; set; }
        DateTime CreatedDate { get; set; }
        Guid CreatedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
        Guid? ModifiedBy { get; set; }
        bool IsDeleted { get; set; }
        bool IsActive { get; set; }
        string Title { get; set; }
        string Alias { get; set; }
        string Source { get; set; }
        string Description { get; set; }
        string Keywords { get; set; }
        int OrderId { get; set; }
        DateTime? PublishFrom { get; set; }
        DateTime? PublishTo { get; set; }
        Guid LanguageId { get; set; }
        string LanguageName { get; set; }
        Guid? ImageId { get; set; }
        bool Accepted { get; set; }
        Guid? AcceptedBy { get; set; }
        DateTime? AcceptedWhen { get; set; }
    }
}
