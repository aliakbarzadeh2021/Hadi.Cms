using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class EmployerMapper
    {
        public static List<IEmployerDto> MapToListDto(this List<Employer> instances)
        {
            return Mapper.Map<List<IEmployerDto>>(instances);
        }

        public static List<Employer> MapToEntities(this List<IEmployerDto> instances)
        {
            return Mapper.Map<List<Employer>>(instances);
        }

        public static IEmployerDto MapToDto(this Employer instance)
        {
            return Mapper.Map<IEmployerDto>(instance);
        }

        public static Employer MapToEntity(this IEmployerDto instance)
        {
            return Mapper.Map<Employer>(instance);
        }
    }
}
