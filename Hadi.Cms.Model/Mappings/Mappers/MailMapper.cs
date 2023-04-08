using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;


namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class MailMapper
    {
        public static IMailDto MapToDto(this Mail instance)
        {
            return AutoMapper.Mapper.Map<Mail, IMailDto>(instance);
        }

        public static List<IMailDto> MapToListDto(this List<Mail> instances)
        {
            return AutoMapper.Mapper.Map<List<Mail>, List<IMailDto>>(instances);
        }

        public static Mail MaptoEntity(this IMailDto instance)
        {
            return AutoMapper.Mapper.Map<IMailDto, Mail>(instance);
        }

        public static List<Mail> MaptoEntities(this List<IMailDto> instances)
        {
            return AutoMapper.Mapper.Map<List<IMailDto>, List<Mail>>(instances);
        }
    }
}
