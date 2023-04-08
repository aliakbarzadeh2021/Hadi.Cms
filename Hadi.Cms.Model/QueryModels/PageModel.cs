using Hadi.Cms.Language.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Hadi.Cms.Model.QueryModels
{
    public class PageModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string Alias { get; set; }

        [AllowHtml]
        public string Source { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string Keywords { get; set; }
        public int OrderId { get; set; }
        public DateTime? PublishFrom { get; set; }
        public DateTime? PublishTo { get; set; }
        public Guid LanguageId { get; set; }
        public Guid? ImageId { get; set; }
        public bool Accepted { get; set; }
        public Guid? AcceptedBy { get; set; }
        public DateTime? AcceptedWhen { get; set; }
    }
}
