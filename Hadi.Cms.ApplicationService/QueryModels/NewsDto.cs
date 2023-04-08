using Hadi.Cms.Model.Mappings.Interfaces;
using System;
using System.Collections.Generic;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class NewsDto : INewsDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string RuTitr { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Source { get; set; }
        public Guid? ThumbnailImage { get; set; }
        public Guid? Image { get; set; }
        public Guid? MainTitrImage { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool IsPublished { get; set; }
        public bool IsMainTitr { get; set; }
        public bool IsHotLink { get; set; }
        public bool WithImage { get; set; }
        public bool WithFilm { get; set; }
        public bool WithVoice { get; set; }
        public bool IsLiveNews { get; set; }
        public DateTime ShowPriorityDate { get; set; }
        public Guid LanguageId { get; set; }
        public ICollection<INewsNewsCategory> NewsNewsCategories { get; set; }
        public string CategoriesTitle { get; set; }
    }
}
