using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class CareerOpportunityMapper
    {
        public static List<ICareerOpportunityDto> MapToListDto(this List<CareerOpportunity> instances)
        {
            return Mapper.Map<List<ICareerOpportunityDto>>(instances);
        }

        public static List<CareerOpportunity> MapToEntities(this List<ICareerOpportunityDto> instances)
        {
            return Mapper.Map<List<CareerOpportunity>>(instances);
        }

        public static ICareerOpportunityDto MapToDto(this CareerOpportunity instance)
        {
            return Mapper.Map<ICareerOpportunityDto>(instance);
        }

        public static CareerOpportunity MapToEntity(this ICareerOpportunityDto instance)
        {
            return Mapper.Map<CareerOpportunity>(instance);
        }
    }
}
