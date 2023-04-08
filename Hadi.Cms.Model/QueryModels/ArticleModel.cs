using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Hadi.Cms.Model.QueryModels
{
    public class ArticleModel
    {
        public Guid Id { get; set; }
        public Guid? AuthorId { get; set; }
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Summary { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Source { get; set; }

        public string ImageCaption { get; set; }
        public Guid? AttachmentImageId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public bool IsSpecial { get; set; }
        public List<Guid> Tags { get; set; }
        public Guid LanguageId { get; set; }

    }
}
