using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class EventLogMapper
    {
        public static IEventLogDto MapToDto(this EventLog instance)
        {
            return AutoMapper.Mapper.Map<EventLog, IEventLogDto>(instance);
        }

        public static List<IEventLogDto> MapToListDto(this List<EventLog> instances)
        {
            return AutoMapper.Mapper.Map<List<EventLog>, List<IEventLogDto>>(instances);
        }

        public static EventLog MaptoEntity(this IEventLogDto instance)
        {
            return AutoMapper.Mapper.Map<IEventLogDto, EventLog>(instance);
        }

        public static List<EventLog> MaptoEntities(this List<IEventLogDto> instances)
        {
            return AutoMapper.Mapper.Map<List<IEventLogDto>, List<EventLog>>(instances);
        }
    }
}
