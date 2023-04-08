using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class DepartmentServiceMapper
    {
        public static List<IDepartmentServiceDto> MapToListDto(this List<DepartmentService> instances)
        {
            return Mapper.Map<List<IDepartmentServiceDto>>(instances);
        }

        public static List<DepartmentService> MapToListEntity(this List<IDepartmentServiceDto> instances)
        {
            return Mapper.Map<List<DepartmentService>>(instances);
        }

        public static IDepartmentServiceDto MapToDto(this DepartmentService instance)
        {
            return Mapper.Map<IDepartmentServiceDto>(instance);
        }

        public static DepartmentService MapToEntity(this IDepartmentServiceDto instance)
        {
            return Mapper.Map<DepartmentService>(instance);
        }
    }
}
