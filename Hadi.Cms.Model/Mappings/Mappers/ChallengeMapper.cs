using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class ChallengeMapper
    {
        public static List<IChallengeDto> MapToListDto(this List<Challenge> instances)
        {
            return Mapper.Map<List<IChallengeDto>>(instances);
        }

        public static IChallengeDto MapToDto(this Challenge instance)
        {
            return Mapper.Map<IChallengeDto>(instance);
        }

        public static List<Challenge> MapToEntities(this List<IChallengeDto> instances)
        {
            return Mapper.Map<List<Challenge>>(instances);
        }

        public static Challenge MapToEntity(this IChallengeDto instance)
        {
            return Mapper.Map<Challenge>(instance);
        }
    }
}
