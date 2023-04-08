using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class NlMessageEmailMapper
    {
        public static List<INlMessageEmailDto> MapToListDto(this List<NlMessageEmail> instances)
        {
            return Mapper.Map<List<INlMessageEmailDto>>(instances);
        }

        public static List<NlMessageEmail> MapToEntities(this List<INlMessageEmailDto> instances)
        {
            return Mapper.Map<List<NlMessageEmail>>(instances);
        }

        public static INlMessageEmailDto MapToDto(this NlMessageEmail instance)
        {
            return Mapper.Map<INlMessageEmailDto>(instance);
        }

        public static NlMessageEmail MapToEntity(this INlMessageEmailDto instance)
        {
            return Mapper.Map<NlMessageEmail>(instance);
        }
    }
}
