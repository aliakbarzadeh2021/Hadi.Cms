using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface INewsDto
    {
        Guid Id { get; set; }
        DateTime CreatedDate { get; set; }
        Guid CreatedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
        Guid? ModifiedBy { get; set; }
        bool IsDeleted { get; set; }
        bool IsActive { get; set; }
        string RuTitr { get; set; }
        string Title { get; set; }
        string SubTitle { get; set; }
        string Source { get; set; }
        Guid? ThumbnailImage { get; set; }
        Guid? Image { get; set; }
        Guid? MainTitrImage { get; set; }
        DateTime ReleaseDate { get; set; }
        bool IsPublished { get; set; }
        bool IsMainTitr { get; set; }
        bool IsHotLink { get; set; }
        bool WithImage { get; set; }
        bool WithFilm { get; set; }
        bool WithVoice { get; set; }
        bool IsLiveNews { get; set; }
        DateTime ShowPriorityDate { get; set; }
        Guid LanguageId { get; set; }
        ICollection<INewsNewsCategory> NewsNewsCategories { get; set; }
    }
}
