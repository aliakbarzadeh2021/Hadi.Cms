using Hadi.Cms.Model.Mappings.Interfaces;
using System;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class RoleFeatureDto : IRoleFeatureDto
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public Guid FeatureId { get; set; }
        public IFeatureDto FeatureDto { get; set; }
        public IRoleDto RoleDto { get; set; }
    }
}
