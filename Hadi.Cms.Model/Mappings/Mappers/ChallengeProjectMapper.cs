using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public  static class ChallengeProjectMapper
    {
        public static List<IChallengeProjectDto> MapToListDto(this List<ChallengeProject> instances)
        {
            return Mapper.Map<List<IChallengeProjectDto>>(instances);
        }

        public static IChallengeProjectDto MapToDto(this ChallengeProject instance)
        {
            return Mapper.Map<IChallengeProjectDto>(instance);
        }

        public static List<ChallengeProject> MapToEntities(this List<IChallengeProjectDto> instances)
        {
            return Mapper.Map<List<ChallengeProject>>(instances);
        }

        public static ChallengeProject MapToEntity(this IChallengeProjectDto instance)
        {
            return Mapper.Map<ChallengeProject>(instance);
        }
    }
}
