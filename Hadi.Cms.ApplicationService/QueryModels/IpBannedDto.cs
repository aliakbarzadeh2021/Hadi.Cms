using Hadi.Cms.Language.Resources;
using Hadi.Cms.Model.Mappings.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class IpBannedDto : IIpBannedDto
    {
        public Guid Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "IpBannedModel_IPAddressBanReason")]
        public string IpAddressBanReason { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "IpBannedModel_IPAddress")]
        public string IpAddress { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "IpBannedModel_IsActive")]
        public bool IsActive { get; set; }

    }
}
