using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Entities
{
    public class News : BaseModel
    {
        public News()
        {
            NewsNewsCategories = new HashSet<NewsNewsCategory>();
        }

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
        public ICollection<NewsNewsCategory> NewsNewsCategories { get; set; }
    }
}
