using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hadi.Cms.Model.Entities
{
    public class RelatedArticle : BaseModel
    {
        public RelatedArticle()
        {
        }

        public Guid SourceId { get; set; }
        public Guid DestinationId { get; set; }

        [ForeignKey("SourceId")]
        public virtual Article SourceArticle { get; set; }

        [ForeignKey("DestinationId")]
        public virtual Article DestinationArticle { get; set; }
    }
}
