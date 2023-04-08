using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class ProvinceMapper
    {
        public static IProvinceDto MapToDto(this Province instance)
        {
            return AutoMapper.Mapper.Map<Province, IProvinceDto>(instance);
        }

        public static List<IProvinceDto> MapToListDto(this List<Province> instances)
        {
            return AutoMapper.Mapper.Map<List<Province>, List<IProvinceDto>>(instances);
        }

        public static Province MaptoEntity(this IProvinceDto instance)
        {
            return AutoMapper.Mapper.Map<IProvinceDto, Province>(instance);
        }

        public static List<Province> MaptoEntities(this List<IProvinceDto> instances)
        {
            return AutoMapper.Mapper.Map<List<IProvinceDto>, List<Province>>(instances);
        }
    }
}
