using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class ChallengeTagMapper
    {
        public static List<IChallengeTagDto> MapToListDto(this List<ChallengeTag> instances)
        {
            return Mapper.Map<List<IChallengeTagDto>>(instances);
        }

        public static List<Challenge> MapToEntities(this List<IChallengeTagDto> instances)
        {
            return Mapper.Map<List<Challenge>>(instances);
        }

        public static IChallengeTagDto MapToDto(this ChallengeTag instance)
        {
            return Mapper.Map<IChallengeTagDto>(instance);
        }

        public static ChallengeTag MapToEntity(this IChallengeTagDto instance)
        {
            return Mapper.Map<ChallengeTag>(instance);
        }

    }
}
