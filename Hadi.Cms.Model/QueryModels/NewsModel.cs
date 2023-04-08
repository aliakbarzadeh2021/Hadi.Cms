using Hadi.Cms.Language.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hadi.Cms.Model.QueryModels
{
    public class NewsModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        public Guid Guid { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NewsModel_RuTitr")]
        public string RuTitr { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NewsModel_Title")]
        public string Title { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NewsModel_SubTitle")]
        public string SubTitle { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NewsModel_Source")]
        public string Source { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NewsModel_ThumbnailImage")]
        public Guid? ThumbnailImage { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NewsModel_NewsImage")]
        public Guid? Image { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NewsModel_MainTitrImage")]
        public Guid? MainTitrImage { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NewsModel_NewsReleaseDate")]
        //[DisplayFormat(DataFormatString = "{0: MM/dd/yyyy HH:mm:ss}")]
        public string ReleaseDate { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NewsModel_IsPublished")]
        public bool IsPublished { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NewsModel_IsMainTitr")]
        public bool IsMainTitr { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NewsModel_IsHotLink")]
        public bool IsHotLink { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NewsModel_WithImage")]
        public bool WithImage { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NewsModel_WithFilm")]
        public bool WithFilm { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NewsModel_WithVoice")]
        public bool WithVoice { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NewsModel_IsLiveNews")]
        public bool IsLiveNews { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NewsModel_NewsShowPriorityDate")]
        //[DisplayFormat(DataFormatString = "{0: MM/dd/yyyy HH:mm:ss}")]
        public string ShowPriorityDate { get; set; }

        public List<Guid> CategoriesId { get; set; }
        public Guid LanguageId { get; set; }
    }
}
