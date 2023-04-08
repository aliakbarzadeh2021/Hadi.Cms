using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class FormMapper
    {
        public static List<IFormDto> MapToListDto(this List<Form> instances)
        {
            return Mapper.Map<List<IFormDto>>(instances);
        }

        public static List<Form> MapToEntities(this List<IFormDto> instances)
        {
            return Mapper.Map<List<Form>>(instances);
        }

        public static IFormDto MapToDto(this Form instance)
        {
            return Mapper.Map<IFormDto>(instance);
        }

        public static Form MapToEntity(this IFormDto instance)
        {
            return Mapper.Map<Form>(instance);
        }
    }
}
