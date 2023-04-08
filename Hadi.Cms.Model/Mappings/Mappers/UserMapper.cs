using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class UserMapper
    {
        public static IUserDto MapToDto(this User instance)
        {
            return AutoMapper.Mapper.Map<User, IUserDto>(instance);
        }

        public static List<IUserDto> MapToListDto(this List<User> instances)
        {
            return AutoMapper.Mapper.Map<List<User>, List<IUserDto>>(instances);
        }

        public static User MaptoEntity(this IUserDto instance)
        {
            return AutoMapper.Mapper.Map<IUserDto, User>(instance);
        }

        public static List<User> MaptoEntities(this List<IUserDto> instances)
        {
            return AutoMapper.Mapper.Map<List<IUserDto>, List<User>>(instances);
        }
    }
}
