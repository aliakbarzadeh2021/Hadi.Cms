using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class TemplateMapper
    {
        public static ITemplateDto MapToDto(this Template instance)
        {
            return AutoMapper.Mapper.Map<Template, ITemplateDto>(instance);
        }

        public static List<ITemplateDto> MapToListDto(this List<Template> instances)
        {
            return AutoMapper.Mapper.Map<List<Template>, List<ITemplateDto>>(instances);
        }

        public static Template MaptoEntity(this ITemplateDto instance)
        {
            return AutoMapper.Mapper.Map<ITemplateDto, Template>(instance);
        }

        public static List<Template> MaptoEntities(this List<ITemplateDto> instances)
        {
            return AutoMapper.Mapper.Map<List<ITemplateDto>, List<Template>>(instances);
        }
    }
}
