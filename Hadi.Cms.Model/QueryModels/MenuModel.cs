using System;

namespace Hadi.Cms.Model.QueryModels
{
    public class MenuModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Target { get; set; }
        public string Relationship { get; set; }
        public string CssClass { get; set; }
        public int OrderId { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public Guid LanguageId { get; set; }
        public Guid? ImageId { get; set; }
        public Guid? ParentId { get; set; }
        public bool IsSideBar { get; set; }
        public string Color { get; set; }
        public bool IsParent { get; set; }
    }
}
