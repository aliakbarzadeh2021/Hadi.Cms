using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class ServiceMapper
    {
        public static List<IServiceDto> MapToListDto(this List<Service> instances)
        {
            return Mapper.Map<List<IServiceDto>>(instances);
        }

        public static List<Service> MapToEntities(this List<IServiceDto> instances)
        {
            return Mapper.Map<List<Service>>(instances);
        }

        public static IServiceDto MapToDto(this Service instance)
        {
            return Mapper.Map<IServiceDto>(instance);
        }

        public static Service MapToEntity(this IServiceDto instance)
        {
            return Mapper.Map<Service>(instance);
        }
    }
}
