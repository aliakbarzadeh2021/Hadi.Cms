using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class EventMapper
    {
        public static List<IEventDto> MapToListDto(this List<Event> instances)
        {
            return Mapper.Map<List<IEventDto>>(instances);
        }

        public static List<Event> MapToEntities(this List<IEventDto> instances)
        {
            return Mapper.Map<List<Event>>(instances);
        }

        public static IEventDto MapToDto(this Event instance)
        {
            return Mapper.Map<IEventDto>(instance);
        }

        public static Event MapToEntity(this IEventDto instance)
        {
            return Mapper.Map<Event>(instance);
        }
    }
}
