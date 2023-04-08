using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class FormValueMapper
    {
        public static List<IFormFieldValueDto> MapToListDto(this List<FormFieldValue> instances)
        {
            return Mapper.Map<List<IFormFieldValueDto>>(instances);
        }

        public static List<FormFieldValue> MapToEntities(this List<IFormFieldValueDto> instances)
        {
            return Mapper.Map<List<FormFieldValue>>(instances);
        }

        public static IFormFieldValueDto MapToDto(this FormFieldValue instance)
        {
            return Mapper.Map<IFormFieldValueDto>(instance);
        }

        public static FormFieldValue MapToEntity(this IFormFieldValueDto instance)
        {
            return Mapper.Map<FormFieldValue>(instance);
        }
    }
}
