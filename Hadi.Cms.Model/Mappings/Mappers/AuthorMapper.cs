using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class AuthorMapper
    {
        public static List<IAuthorDto> MapToListDto(this List<Author> instances)
        {
            return Mapper.Map<List<IAuthorDto>>(instances);
        }

        public static List<Author> MapToEntities(this List<IAuthorDto> instances)
        {
            return Mapper.Map<List<Author>>(instances);
        }

        public static IAuthorDto MapToDto(this Author instance)
        {
            return Mapper.Map<IAuthorDto>(instance);
        }

        public static Author MapToEntity(this IAuthorDto instance)
        {
            return Mapper.Map<Author>(instance);
        }
    }
}
