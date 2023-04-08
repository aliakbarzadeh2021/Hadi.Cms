using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class LoginHistoryMapper
    {
        public static ILoginHistoryDto MapToDto(this LoginHistory instance)
        {
            return AutoMapper.Mapper.Map<LoginHistory, ILoginHistoryDto>(instance);
        }

        public static List<ILoginHistoryDto> MapToListDto(this List<LoginHistory> instances)
        {
            return AutoMapper.Mapper.Map<List<LoginHistory>, List<ILoginHistoryDto>>(instances);
        }

        public static LoginHistory MaptoEntity(this ILoginHistoryDto instance)
        {
            return AutoMapper.Mapper.Map<ILoginHistoryDto, LoginHistory>(instance);
        }

        public static List<LoginHistory> MaptoEntities(this List<ILoginHistoryDto> instances)
        {
            return AutoMapper.Mapper.Map<List<ILoginHistoryDto>, List<LoginHistory>>(instances);
        }
    }
}
