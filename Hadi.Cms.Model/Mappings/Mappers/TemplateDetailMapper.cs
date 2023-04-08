using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class TemplateDetailMapper
    {
        public static ITemplateDetailDto MapToDto(this TemplateDetail instance)
        {
            return AutoMapper.Mapper.Map<TemplateDetail, ITemplateDetailDto>(instance);
        }

        public static List<ITemplateDetailDto> MapToListDto(this List<TemplateDetail> instances)
        {
            return AutoMapper.Mapper.Map<List<TemplateDetail>, List<ITemplateDetailDto>>(instances);
        }

        public static TemplateDetail MaptoEntity(this ITemplateDetailDto instance)
        {
            return AutoMapper.Mapper.Map<ITemplateDetailDto, TemplateDetail>(instance);
        }

        public static List<TemplateDetail> MaptoEntities(this List<ITemplateDetailDto> instances)
        {
            return AutoMapper.Mapper.Map<List<ITemplateDetailDto>, List<TemplateDetail>>(instances);
        }
    }
}
