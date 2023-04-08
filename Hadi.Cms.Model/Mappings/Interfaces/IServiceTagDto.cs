using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IServiceTagDto
    {
        Guid Id { get; set; }
        Guid ServiceId { get; set; }
        Guid TagId { get; set; }
        IServiceDto ServiceDto { get; set; }
        ITagDto TagDto { get; set; }
    }
}
