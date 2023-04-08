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
    public static class ServiceCommentMapper
    {
        public static List<IServiceCommentDto> MapToListDto(this List<ServiceComment> instances)
        {
            return Mapper.Map<List<IServiceCommentDto>>(instances);
        }

        public static List<ServiceComment> MapToEntities(this List<IServiceCommentDto> instances)
        {
            return Mapper.Map<List<ServiceComment>>(instances);
        }

        public static IServiceCommentDto MapToDto(this ServiceComment instance)
        {
            return Mapper.Map<IServiceCommentDto>(instance);
        }

        public static ServiceComment MapToEntity(this IServiceCommentDto instance)
        {
            return Mapper.Map<ServiceComment>(instance);
        }
    }
}
