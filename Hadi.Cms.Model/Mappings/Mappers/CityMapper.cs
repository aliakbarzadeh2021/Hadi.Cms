using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class CityMapper
    {
        public static ICityDto MapToDto(this City instance)
        {
            return AutoMapper.Mapper.Map<City, ICityDto>(instance);
        }

        public static List<ICityDto> MapToListDto(this List<City> instances)
        {
            return AutoMapper.Mapper.Map<List<City>, List<ICityDto>>(instances);
        }

        public static City MaptoEntity(this ICityDto instance)
        {
            return AutoMapper.Mapper.Map<ICityDto, City>(instance);
        }

        public static List<City> MaptoEntities(this List<ICityDto> instances)
        {
            return AutoMapper.Mapper.Map<List<ICityDto>, List<City>>(instances);
        }
    }
}
