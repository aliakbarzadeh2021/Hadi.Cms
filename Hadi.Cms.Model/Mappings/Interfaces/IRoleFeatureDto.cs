using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IRoleFeatureDto
    {
        Guid Id { get; set; }
        Guid RoleId { get; set; }
        Guid FeatureId { get; set; }
        IFeatureDto FeatureDto { get; set; }
        IRoleDto RoleDto { get; set; }
    }
}
