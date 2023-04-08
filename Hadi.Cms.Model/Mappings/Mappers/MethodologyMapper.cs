using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class MethodologyMapper
    {
        public static List<IMethodologyDto> MapToListDto(this List<Methodology> instances)
        {
            return Mapper.Map<List<IMethodologyDto>>(instances);
        }

        public static List<Methodology> MapToEntities(this List<IMethodologyDto> instances)
        {
            return Mapper.Map<List<Methodology>>(instances);
        }

        public static IMethodologyDto MapToDto(this Methodology instance)
        {
            return Mapper.Map<IMethodologyDto>(instance);
        }

        public static Methodology MapToEntity(this IMethodologyDto instance)
        {
            return Mapper.Map<Methodology>(instance);
        }
    }
}
