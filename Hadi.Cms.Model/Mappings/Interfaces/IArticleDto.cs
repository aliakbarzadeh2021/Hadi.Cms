using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IArticleDto : IBaseModelDto
    {
        Guid? AuthorId { get; set; }
        string Title { get; set; }
        string Summary { get; set; }
        string Source { get; set; }
        string ImageCaption { get; set; }
        int ReviewCount { get; set; }
        string AuthorName { get; set; }
        Guid? AuthorImageGuid { get; set; }
        string AuthorImageSource { get; set; }
        Guid? AttachmentImageId { get; set; }
        string AttachmentImageSource { get; set; }
        Guid LanguageId { get; set; }
        bool IsSpecial { get; set; }
        DateTime? IsSpecialCreatedDate { get; set; }
        ICollection<IArticleTagDto> ArticleTagsDto { get; set; }
        IAuthorDto AuthorDto { get; set; }
    }
}
