using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IIpRangeDto
    {
        Guid Id { get; set; }
        Guid? CreatedBy { get; set; }
        DateTime? CreateDate { get; set; }
        string Lower { get; set; }
        string Upper { get; set; }
        bool IsActive { get; set; }
    }
}
