using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class TagMapper
    {
        public static ITagDto MapToDto(this Tag instance)
        {
            return AutoMapper.Mapper.Map<Tag, ITagDto>(instance);
        }

        public static List<ITagDto> MapToListDto(this List<Tag> instances)
        {
            return AutoMapper.Mapper.Map<List<Tag>, List<ITagDto>>(instances);
        }

        public static Tag MaptoEntity(this ITagDto instance)
        {
            return AutoMapper.Mapper.Map<ITagDto, Tag>(instance);
        }

        public static List<Tag> MaptoEntities(this List<ITagDto> instances)
        {
            return AutoMapper.Mapper.Map<List<ITagDto>, List<Tag>>(instances);
        }
    }
}
