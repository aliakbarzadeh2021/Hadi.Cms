using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class RoleMapper
    {
        public static IRoleDto MapToDto(this Role instance)
        {
            return AutoMapper.Mapper.Map<Role, IRoleDto>(instance);
        }

        public static List<IRoleDto> MapToListDto(this List<Role> instances)
        {
            return AutoMapper.Mapper.Map<List<Role>, List<IRoleDto>>(instances);
        }

        public static Role MaptoEntity(this IRoleDto instance)
        {
            return AutoMapper.Mapper.Map<IRoleDto, Role>(instance);
        }

        public static List<Role> MaptoEntities(this List<IRoleDto> instances)
        {
            return AutoMapper.Mapper.Map<List<IRoleDto>, List<Role>>(instances);
        }

    }
}
