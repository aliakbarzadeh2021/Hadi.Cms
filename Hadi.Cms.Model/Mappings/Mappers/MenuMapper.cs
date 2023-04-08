using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class MenuMapper
    {
        public static IMenuDto MapToDto(this Menu instance)
        {
            return AutoMapper.Mapper.Map<Menu, IMenuDto>(instance);
        }

        public static List<IMenuDto> MapToListDto(this List<Menu> instances)
        {
            return AutoMapper.Mapper.Map<List<Menu>, List<IMenuDto>>(instances);
        }

        public static Menu MaptoEntity(this IMenuDto instance)
        {
            return AutoMapper.Mapper.Map<IMenuDto, Menu>(instance);
        }

        public static List<Menu> MaptoEntities(this List<IMenuDto> instances)
        {
            return AutoMapper.Mapper.Map<List<IMenuDto>, List<Menu>>(instances);
        }
    }
}
