using System;
using System.Collections.Generic;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IDepartmentServiceDto
    {
        Guid Id { get; set; }
        Guid DepartmentId { get; set; }
        Guid ServiceId { get; set; }
        DateTime CreatedDate { get; set; }
        bool IsActive { get; set; }
        IDepartmentDto DepartmentDto { get; set; }
        IServiceDto ServiceDto { get; set; }
    }
}
