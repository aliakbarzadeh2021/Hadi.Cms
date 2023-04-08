using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Hadi.Cms.Model.Entities
{
    public class Template : BaseModel
    {
        public Template()
        {
            TemplateDetails = new HashSet<TemplateDetail>();
        }

        public string Title { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Source { get; set; }

        public virtual ICollection<TemplateDetail> TemplateDetails { get; set; }
    }
}
