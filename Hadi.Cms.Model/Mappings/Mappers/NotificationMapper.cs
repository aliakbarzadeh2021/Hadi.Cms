using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class NotificationMapper
    {
        public static INotificationDto MapToDto(this Notification instance)
        {
            return AutoMapper.Mapper.Map<Notification, INotificationDto>(instance);
        }

        public static List<INotificationDto> MapToListDto(this List<Notification> instances)
        {
            return AutoMapper.Mapper.Map<List<Notification>, List<INotificationDto>>(instances);
        }

        public static Notification MaptoEntity(this INotificationDto instance)
        {
            return AutoMapper.Mapper.Map<INotificationDto, Notification>(instance);
        }

        public static List<Notification> MaptoEntities(this List<INotificationDto> instances)
        {
            return AutoMapper.Mapper.Map<List<INotificationDto>, List<Notification>>(instances);
        }
    }
}
