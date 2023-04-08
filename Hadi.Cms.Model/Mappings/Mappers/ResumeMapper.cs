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
    public static class ResumeMapper
    {
        public static List<IResumeDto> MapToListDto(this List<Resume> instances)
        {
            return Mapper.Map<List<IResumeDto>>(instances);
        }

        public static List<Resume> MapToEntities(this List<IResumeDto> instances)
        {
            return Mapper.Map<List<Resume>>(instances);
        }

        public static IResumeDto MapToDto(this Resume instance)
        {
            return Mapper.Map<IResumeDto>(instance);
        }

        public static Resume MapToEntity(this IResumeDto instance)
        {
            return Mapper.Map<Resume>(instance);
        }
    }
}
