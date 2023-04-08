using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class NlMessageMapper
    {
        public static List<INlMessageDto> MapToListDto(this List<NlMessage> instances)
        {
            return Mapper.Map<List<INlMessageDto>>(instances);
        }

        public static List<NlMessage> MapToEntities(this List<INlMessageDto> instances)
        {
            return Mapper.Map<List<NlMessage>>(instances);
        }

        public static INlMessageDto MapToDto(this NlMessage instance)
        {
            return Mapper.Map<INlMessageDto>(instance);
        }

        public static NlMessage MapToEntity(this INlMessageDto instance)
        {
            return Mapper.Map<NlMessage>(instance);
        }
    }
}
