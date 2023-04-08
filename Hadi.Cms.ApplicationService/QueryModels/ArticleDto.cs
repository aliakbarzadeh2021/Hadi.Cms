using Hadi.Cms.Model.Mappings.Interfaces;
using System;
using System.Collections.Generic;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class ArticleDto : IArticleDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Source { get; set; }
        public string ImageCaption { get; set; }
        public int ReviewCount { get; set; }
        public string AuthorName { get; set; }
        public Guid? AuthorImageGuid { get; set; }
        public string AuthorImageSource { get; set; }
        public Guid? AttachmentImageId { get; set; }
        public string AttachmentImageSource { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Guid LanguageId { get; set; }
        public bool IsSpecial { get; set; }
        public DateTime? IsSpecialCreatedDate { get; set; }

        public IAttachmentFileDto AttachmentFileDto { get; set; }
        public ICollection<IArticleTagDto> ArticleTagsDto { get; set; }
        public Guid? AuthorId { get; set; }
        public IAuthorDto AuthorDto { get; set; }
    }
}
