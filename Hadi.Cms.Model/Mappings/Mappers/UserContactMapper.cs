using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class UserContactMapper
    {
        public static IUserContactDto MapToDto(this UserContact instance)
        {
            return AutoMapper.Mapper.Map<UserContact, IUserContactDto>(instance);
        }

        public static List<IUserContactDto> MapToListDto(this List<UserContact> instances)
        {
            return AutoMapper.Mapper.Map<List<UserContact>, List<IUserContactDto>>(instances);
        }

        public static UserContact MaptoEntity(this IUserContactDto instance)
        {
            return AutoMapper.Mapper.Map<IUserContactDto, UserContact>(instance);
        }

        public static List<UserContact> MaptoEntities(this List<IUserContactDto> instances)
        {
            return AutoMapper.Mapper.Map<List<IUserContactDto>, List<UserContact>>(instances);
        }

    }
}
