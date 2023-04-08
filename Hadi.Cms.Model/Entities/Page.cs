using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Hadi.Cms.Model.Entities
{
    public class Page : BaseModel
    {
        public Page()
        {
        }

        public string Title { get; set; }
        public string Alias { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
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
