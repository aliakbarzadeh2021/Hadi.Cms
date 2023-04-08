using Hadi.Cms.Model.Mappings.Interfaces;
using System;
using System.Collections.Generic;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class TagDto : ITagDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string UniqueValue { get; set; }
        public Guid? ParentId { get; set; }
        public string ParentTitle { get; set; }
        public string Color { get; set; }
        public int OrderId { get; set; }
        public int CountOfUse { get; set; }
        public int CountOfAllTags { get; set; }
        public ICollection<IArticleTagDto> ArticleTagsDto { get; set; }
        public ICollection<ICourseTagDto> CourseTagDto { get; set; }
        public ICollection<IServiceTagDto> ServiceTagDto { get; set; }
        public ICollection<IAttachmentFileTagDto> AttachmentFileTagDto { get; set; }
    }
}
