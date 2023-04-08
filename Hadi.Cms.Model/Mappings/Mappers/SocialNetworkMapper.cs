using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class SocialNetworkMapper
    {
        public static List<ISocialNetwork> MapToDto(this List<SocialNetwork> instance)
        {
            return Mapper.Map<List<ISocialNetwork>>(instance);
        }

        public static List<SocialNetwork> MapToEntities(this List<ISocialNetwork> instance)
        {
            return Mapper.Map<List<SocialNetwork>>(instance);
        }

        public static ISocialNetwork MapToDto(this SocialNetwork instance)
        {
            return Mapper.Map<ISocialNetwork>(instance);
        }

        public static SocialNetwork MapToEntity(this ISocialNetwork instance)
        {
            return Mapper.Map<SocialNetwork>(instance);
        }
    }
}
