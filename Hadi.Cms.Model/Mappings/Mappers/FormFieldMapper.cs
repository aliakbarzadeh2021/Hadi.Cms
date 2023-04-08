using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;
using AutoMapper;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class FormFieldMapper
    {
        public static List<IFormFieldDto> MapToListDto(this List<FormField> instances)
        {
            return Mapper.Map<List<IFormFieldDto>>(instances);
        }

        public static List<FormField> MapToEntities(this List<IFormFieldDto> instances)
        {
            return Mapper.Map<List<FormField>>(instances);
        }

        public static IFormFieldDto MapToDto(this FormField instance)
        {
            return Mapper.Map<IFormFieldDto>(instance);
        }

        public static FormField MapToEntity(this IFormFieldDto instance)
        {
            return Mapper.Map<FormField>(instance);
        }
    }
}
