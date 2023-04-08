using System.Collections.Generic;
using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Model.Mappings.Mappers
{
    public static class SliderMapper
    {
        public static List<ISliderDto> MapToListDto(this List<Slider> instances)
        {
            return Mapper.Map<List<ISliderDto>>(instances);
        }

        public static List<Slider> MapToEntities(this List<ISliderDto> instances)
        {
            return Mapper.Map<List<Slider>>(instances);
        }

        public static ISliderDto MapToDto(this Slider instance)
        {
            return Mapper.Map<ISliderDto>(instance);
        }

        public static Slider MapToEntity(this ISliderDto instance)
        {
            return Mapper.Map<Slider>(instance);
        }
    }
}
