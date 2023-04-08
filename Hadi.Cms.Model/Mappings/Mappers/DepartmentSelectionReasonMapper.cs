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
    public static class DepartmentSelectionReasonMapper
    {
        public static List<IDepartmentSelectionReasonDto> MapToListDto(this List<DepartmentSelectionReason> instances)
        {
            return Mapper.Map<List<IDepartmentSelectionReasonDto>>(instances);
        }

        public static List<DepartmentSelectionReason> MapToEntities(this List<IDepartmentSelectionReasonDto> instances)
        {
            return Mapper.Map<List<DepartmentSelectionReason>>(instances);
        }

        public static IDepartmentSelectionReasonDto MapToDto(this DepartmentSelectionReason instance)
        {
            return Mapper.Map<IDepartmentSelectionReasonDto>(instance);
        }

        public static DepartmentSelectionReason MapToEntity(this IDepartmentSelectionReasonDto instance)
        {
            return Mapper.Map<DepartmentSelectionReason>(instance);
        }
    }
}
