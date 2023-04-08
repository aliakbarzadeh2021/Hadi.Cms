using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IFeatureDto
    {
        Guid Id { get; set; }
        string AreaName { get; set; }
        string ControllerName { get; set; }
        string ActionName { get; set; }
        string Attributes { get; set; }
        string FeaturesName { get; set; }
        ICollection<IRoleFeatureDto> RoleFeaturesDto { get; set; }
    }
}
