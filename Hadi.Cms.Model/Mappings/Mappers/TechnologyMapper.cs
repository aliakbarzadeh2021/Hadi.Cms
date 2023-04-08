using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class TechnologyMapper
    {
        public static List<ITechnologyDto> MapToListDto(this List<Technology> instances)
        {
            return Mapper.Map<List<ITechnologyDto>>(instances);
        }

        public static List<Technology> MapToEntities(this List<ITechnologyDto> instances)
        {
            return Mapper.Map<List<Technology>>(instances);
        }

        public static ITechnologyDto MapToDto(this Technology instance)
        {
            return Mapper.Map<ITechnologyDto>(instance);
        }

        public static Technology MapToEntity(this ITechnologyDto instance)
        {
            return Mapper.Map<Technology>(instance);
        }
    }
}
