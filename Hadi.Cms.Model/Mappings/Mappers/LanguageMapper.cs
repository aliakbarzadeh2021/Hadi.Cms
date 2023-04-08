using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class LanguageMapper
    {
        public static ILanguageDto MapToDto(this Entities.Language instance)
        {
            return AutoMapper.Mapper.Map<Entities.Language, ILanguageDto>(instance);
        }

        public static List<ILanguageDto> MapToListDto(this List<Entities.Language> instances)
        {
            return AutoMapper.Mapper.Map<List<Entities.Language>, List<ILanguageDto>>(instances);
        }

        public static Entities.Language MaptoEntity(this ILanguageDto instance)
        {
            return AutoMapper.Mapper.Map<ILanguageDto, Entities.Language>(instance);
        }

        public static List<Entities.Language> MaptoEntities(this List<ILanguageDto> instances)
        {
            return AutoMapper.Mapper.Map<List<ILanguageDto>, List<Entities.Language>>(instances);
        }
    }
}
