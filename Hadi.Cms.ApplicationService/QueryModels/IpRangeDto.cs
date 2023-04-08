using Hadi.Cms.Language.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class IpRangeDto
    {
        public Guid Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "IpRangeModel_Lower")]
        public string Lower { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "IpRangeModel_Upper")]
        public string Upper { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "IpRangeModel_IsActive")]
        public bool IsActive { get; set; }

    }
}
