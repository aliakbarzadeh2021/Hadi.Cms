using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class ContactUsMapper
    {
        public static IContactUsDto MapToDto(this ContactUs instance)
        {
            return AutoMapper.Mapper.Map<ContactUs, IContactUsDto>(instance);
        }

        public static List<IContactUsDto> MapToListDto(this List<ContactUs> instances)
        {
            return AutoMapper.Mapper.Map<List<ContactUs>, List<IContactUsDto>>(instances);
        }

        public static ContactUs MaptoEntity(this IContactUsDto instance)
        {
            return AutoMapper.Mapper.Map<IContactUsDto, ContactUs>(instance);
        }

        public static List<ContactUs> MaptoEntities(this List<IContactUsDto> instances)
        {
            return AutoMapper.Mapper.Map<List<IContactUsDto>, List<ContactUs>>(instances);
        }
    }
}
