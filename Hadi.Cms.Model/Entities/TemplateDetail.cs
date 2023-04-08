using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Hadi.Cms.Model.Entities
{
    public class TemplateDetail : BaseModel
    {
        public TemplateDetail()
        {
        }

        public Guid TemplateId { get; set; }
        public string Title { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Source { get; set; }

        [ForeignKey("TemplateId")]
        public virtual Template Template { get; set; }
    }
}
