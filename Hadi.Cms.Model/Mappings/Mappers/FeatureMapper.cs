using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class FeatureMapper
    {
        public static IFeatureDto MapToDto(this Feature instance)
        {
            return AutoMapper.Mapper.Map<Feature, IFeatureDto>(instance);
        }

        public static List<IFeatureDto> MapToListDto(this List<Feature> instances)
        {
            return AutoMapper.Mapper.Map<List<Feature>, List<IFeatureDto>>(instances);
        }

        public static Feature MaptoEntity(this IFeatureDto instance)
        {
            return AutoMapper.Mapper.Map<IFeatureDto, Feature>(instance);
        }

        public static List<Feature> MaptoEntities(this List<IFeatureDto> instances)
        {
            return AutoMapper.Mapper.Map<List<IFeatureDto>, List<Feature>>(instances);
        }
    }
}
