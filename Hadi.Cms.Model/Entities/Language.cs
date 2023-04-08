using System;
using System.ComponentModel.DataAnnotations;

namespace Hadi.Cms.Model.Entities
{
    public class Language
    {
        public Language()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public string CultureName { get; set; }
        public string CultureCode { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }
        public bool IsRtl { get; set; }
    }
}
