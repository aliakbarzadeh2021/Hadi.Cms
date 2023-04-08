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
    public static class ServiceReceiverMapper
    {
        public static List<IServiceReceiverDto> MapToListDto(this List<ServiceReceiver> instances)
        {
            return Mapper.Map<List<IServiceReceiverDto>>(instances);
        }

        public static List<ServiceReceiver> MapToEntities(this List<IServiceReceiverDto> instances)
        {
            return Mapper.Map<List<ServiceReceiver>>(instances);
        }

        public static IServiceReceiverDto MapToDto(this ServiceReceiver instance)
        {
            return Mapper.Map<IServiceReceiverDto>(instance);
        }

        public static ServiceReceiver MapToEntity(this IServiceReceiverDto instance)
        {
            return Mapper.Map<ServiceReceiver>(instance);
        }
    }
}
