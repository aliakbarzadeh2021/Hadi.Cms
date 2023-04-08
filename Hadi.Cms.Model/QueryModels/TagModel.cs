using System;
using System.ComponentModel.DataAnnotations;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.Model.QueryModels
{
    public class TagModel
    {
        public Guid Id { get; set; }
        [Required(AllowEmptyStrings = false , ErrorMessageResourceName ="Required",ErrorMessageResourceType = typeof(Strings))]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Strings))]
        public string UniqueValue { get; set; }
        public Guid? ParentId { get; set; }
        public string Color { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

    }
}
