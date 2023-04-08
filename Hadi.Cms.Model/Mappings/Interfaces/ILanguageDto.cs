using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface ILanguageDto
    {
        Guid Id { get; set; }
        string CultureName { get; set; }
        string CultureCode { get; set; }
        string Name { get; set; }
        string DisplayName { get; set; }
        bool IsActive { get; set; }
        bool IsRtl { get; set; }
    }
}
