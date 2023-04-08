using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class EmployeeMapper
    {
        public static List<IEmployeeDto> MapToListDto(this List<Employee> instances)
        {
            return Mapper.Map<List<IEmployeeDto>>(instances);
        }

        public static List<Employee> MapToEntities(this List<IEmployeeDto> instances)
        {
            return Mapper.Map<List<Employee>>(instances);
        }

        public static IEmployeeDto MapToDto(this Employee instance)
        {
            return Mapper.Map<IEmployeeDto>(instance);
        }

        public static Employee MapToEntity(this IEmployeeDto instance)
        {
            return Mapper.Map<Employee>(instance);
        }
    }
}
