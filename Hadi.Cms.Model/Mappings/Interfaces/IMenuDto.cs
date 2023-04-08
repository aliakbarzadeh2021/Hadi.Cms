using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IMenuDto
    {
        Guid Id { get; set; }
        string Title { get; set; }
        string Link { get; set; }
        string Target { get; set; }
        string Relationship { get; set; }
        string CssClass { get; set; }
        int OrderId { get; set; }
        DateTime CreateDate { get; set; }
        Guid CreatedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
        Guid? ModifiedBy { get; set; }
        bool IsActive { get; set; }
        Guid LanguageId { get; set; }
        Guid? ImageId { get; set; }
        Guid? ParentId { get; set; }
        string ParentTitle { get; set; }
        bool IsSideBar { get; set; }
        string Color { get; set; }
        bool IsParent { get; set; }
    }
}
