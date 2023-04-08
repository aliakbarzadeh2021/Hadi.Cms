using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class NlEmailMapper
    {
        public static List<INlEmailDto> MapToListDto(this List<NlEmail> instances)
        {
            return Mapper.Map<List<INlEmailDto>>(instances);
        }

        public static List<NlEmail> MapToEntities(this List<INlEmailDto> instances)
        {
            return Mapper.Map<List<NlEmail>>(instances);
        }

        public static INlEmailDto MapToDto(this NlEmail instance)
        {
            return Mapper.Map<INlEmailDto>(instance);
        }

        public static NlEmail MapToEntity(this INlEmailDto instance)
        {
            return Mapper.Map<NlEmail>(instance);
        }
    }
}
