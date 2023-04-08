using Hadi.Cms.Model.Mappings.Interfaces;
using System;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class LanguageDto : ILanguageDto
    {
        public Guid Id { get; set; }
        public string CultureName { get; set; }
        public string CultureCode { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }
        public bool IsRtl { get; set; }
    }
}
