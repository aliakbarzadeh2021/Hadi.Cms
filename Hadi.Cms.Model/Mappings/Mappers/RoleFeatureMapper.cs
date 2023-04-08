using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class RoleFeatureMapper
    {
        public static IRoleFeatureDto MapToDto(this RoleFeature instance)
        {
            return AutoMapper.Mapper.Map<RoleFeature, IRoleFeatureDto>(instance);
        }

        public static List<IRoleFeatureDto> MapToListDto(this List<RoleFeature> instances)
        {
            return AutoMapper.Mapper.Map<List<RoleFeature>, List<IRoleFeatureDto>>(instances);
        }

        public static RoleFeature MaptoEntity(this IRoleFeatureDto instance)
        {
            return AutoMapper.Mapper.Map<IRoleFeatureDto, RoleFeature>(instance);
        }

        public static List<RoleFeature> MaptoEntities(this List<IRoleFeatureDto> instances)
        {
            return AutoMapper.Mapper.Map<List<IRoleFeatureDto>, List<RoleFeature>>(instances);
        }
    }
}
