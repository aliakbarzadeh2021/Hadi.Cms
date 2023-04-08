using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Hadi.Cms.Model.QueryModels
{
    public class TemplateModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Source { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
