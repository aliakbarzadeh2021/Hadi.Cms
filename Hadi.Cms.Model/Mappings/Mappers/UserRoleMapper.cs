using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class UserRoleMapper
    {
        public static IUserRoleDto MapToDto(this UserRole instance)
        {
            return AutoMapper.Mapper.Map<UserRole, IUserRoleDto>(instance);
        }

        public static List<IUserRoleDto> MapToListDto(this List<UserRole> instances)
        {
            return AutoMapper.Mapper.Map<List<UserRole>, List<IUserRoleDto>>(instances);
        }

        public static UserRole MaptoEntity(this IUserRoleDto instance)
        {
            return AutoMapper.Mapper.Map<IUserRoleDto, UserRole>(instance);
        }

        public static List<UserRole> MaptoEntities(this List<IUserRoleDto> instances)
        {
            return AutoMapper.Mapper.Map<List<IUserRoleDto>, List<UserRole>>(instances);
        }

    }
}
