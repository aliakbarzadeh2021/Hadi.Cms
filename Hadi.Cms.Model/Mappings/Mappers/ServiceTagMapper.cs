using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class ServiceTagMapper
    {
        public static List<IServiceTagDto> MapToListDto(this List<ServiceTag> instances)
        {
            return Mapper.Map<List<IServiceTagDto>>(instances);
        }

        public static List<ServiceTag> MapToListEntities(this List<IServiceTagDto> instances)
        {
            return Mapper.Map<List<ServiceTag>>(instances);
        }

        public static IServiceTagDto MapToDto(this ServiceTag instance)
        {
            return Mapper.Map<IServiceTagDto>(instance);
        }

        public static ServiceTag MapToEntity(this IServiceTagDto instance)
        {
            return Mapper.Map<ServiceTag>(instance);
        }
    }
}
