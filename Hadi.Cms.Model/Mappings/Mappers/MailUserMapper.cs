using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class MailUserMapper
    {
        public static IMailUserDto MapToDto(this MailUser instance)
        {
            return AutoMapper.Mapper.Map<MailUser, IMailUserDto>(instance);
        }

        public static List<IMailUserDto> MapToListDto(this List<MailUser> instances)
        {
            return AutoMapper.Mapper.Map<List<MailUser>, List<IMailUserDto>>(instances);
        }

        public static MailUser MaptoEntity(this IMailUserDto instance)
        {
            return AutoMapper.Mapper.Map<IMailUserDto, MailUser>(instance);
        }

        public static List<MailUser> MaptoEntities(this List<IMailUserDto> instances)
        {
            return AutoMapper.Mapper.Map<List<IMailUserDto>, List<MailUser>>(instances);
        }
    }
}
