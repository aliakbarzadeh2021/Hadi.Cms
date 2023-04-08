using System;
using System.ComponentModel.DataAnnotations;

namespace Hadi.Cms.Model.Entities
{
    public class Menu : BaseModel
    {
        public Menu() { }

        public string Title { get; set; }
        public string Link { get; set; }
        public string Target { get; set; }
        public string Relationship { get; set; }
        public string CssClass { get; set; }
        public int OrderId { get; set; }
        public Guid LanguageId { get; set; }
        public Guid? ImageId { get; set; }
        public Guid? ParentId { get; set; }
        public bool IsSideBar { get; set; }
        public string Color { get; set; }
        public bool IsParent { get; set; }
    }
}
