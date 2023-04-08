using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface ITagDto : IBaseModelDto
    {
        string Title { get; set; }
        string UniqueValue { get; set; }
        Guid? ParentId { get; set; }
        string ParentTitle { get; set; }
        string Color { get; set; }
        int OrderId { get; set; }
        int CountOfUse { get; set; }

        ICollection<IArticleTagDto> ArticleTagsDto { get; set; }
        ICollection<ICourseTagDto> CourseTagDto { get; set; }
        ICollection<IServiceTagDto> ServiceTagDto { get; set; }
        ICollection<IAttachmentFileTagDto> AttachmentFileTagDto { get; set; }
    }
}