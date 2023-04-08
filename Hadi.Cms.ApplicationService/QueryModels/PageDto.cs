using Hadi.Cms.Model.Mappings.Interfaces;
using System;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class PageDto : IPageDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string Alias { get; set; }
        public string Source { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public int OrderId { get; set; }
        public DateTime? PublishFrom { get; set; }
        public DateTime? PublishTo { get; set; }
        public Guid LanguageId { get; set; }
        public string LanguageName { get; set; }
        public Guid? ImageId { get; set; }
        public bool Accepted { get; set; }
        public Guid? AcceptedBy { get; set; }
        public DateTime? AcceptedWhen { get; set; }
    }
}
