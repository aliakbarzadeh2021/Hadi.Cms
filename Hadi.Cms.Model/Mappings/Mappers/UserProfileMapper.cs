using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class UserProfileMapper
    {
        public static IUserProfileDto MapToDto(this UserProfile instance)
        {
            return AutoMapper.Mapper.Map<UserProfile, IUserProfileDto>(instance);
        }

        public static List<IUserProfileDto> MapToListDto(this List<UserProfile> instances)
        {
            return AutoMapper.Mapper.Map<List<UserProfile>, List<IUserProfileDto>>(instances);
        }

        public static UserProfile MaptoEntity(this IUserProfileDto instance)
        {
            return AutoMapper.Mapper.Map<IUserProfileDto, UserProfile>(instance);
        }

        public static List<UserProfile> MaptoEntities(this List<IUserProfileDto> instances)
        {
            return AutoMapper.Mapper.Map<List<IUserProfileDto>, List<UserProfile>>(instances);
        }
    }
}
