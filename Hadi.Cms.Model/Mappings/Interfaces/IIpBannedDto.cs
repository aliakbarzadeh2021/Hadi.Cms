using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IIpBannedDto
    {
        Guid Id { get; set; }
        Guid? CreatedBy { get; set; }
        DateTime? CreateDate { get; set; }
        string IpAddressBanReason { get; set; }
        string IpAddress { get; set; }
        bool IsActive { get; set; }
    }
}
