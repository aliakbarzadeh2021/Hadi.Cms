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
    public static class ServiceReceiverServiceMapper
    {
        public static List<IServiceReceiverServiceDto> MapToListDto(this List<ServiceReceiverService> instances)
        {
            return Mapper.Map<List<IServiceReceiverServiceDto>>(instances);
        }

        public static List<ServiceReceiverService> MapToEntities(this List<IServiceReceiverServiceDto> instances)
        {
            return Mapper.Map<List<ServiceReceiverService>>(instances);
        }

        public static IServiceReceiverServiceDto MapToDto(this ServiceReceiverService instance)
        {
            return Mapper.Map<IServiceReceiverServiceDto>(instance);
        }

        public static ServiceReceiverService MapToEntity(this IServiceReceiverServiceDto instance)
        {
            return Mapper.Map<ServiceReceiverService>(instance);
        }
    }
}
