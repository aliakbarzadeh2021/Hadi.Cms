using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class DepartmentMapper
    {
        public static List<IDepartmentDto> MapToListDto(this List<Department> instances)
        {
            return Mapper.Map<List<IDepartmentDto>>(instances);
        }

        public static List<Department> MapToEntities(this List<IDepartmentDto> instances)
        {
            return Mapper.Map<List<Department>>(instances);
        }

        public static IDepartmentDto MapToDto(this Department instance)
        {
            return Mapper.Map<IDepartmentDto>(instance);
        }

        public static Department MapToEntity(this IDepartmentDto instance)
        {
            return Mapper.Map<Department>(instance);
        }
    }
}
