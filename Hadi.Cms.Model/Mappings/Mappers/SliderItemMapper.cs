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
    public static class SliderItemMapper
    {
        public static List<ISliderItemDto> MapToListDto(this List<SliderItem> instances)
        {
            return Mapper.Map<List<ISliderItemDto>>(instances);
        }

        public static List<SliderItem> MapToEntities(this List<ISliderItemDto> instances)
        {
            return Mapper.Map<List<SliderItem>>(instances);
        }

        public static ISliderItemDto MapToDto(this SliderItem instance)
        {
            return Mapper.Map<ISliderItemDto>(instance);
        }

        public static SliderItem MapToEntity(this ISliderItemDto instance)
        {
            return Mapper.Map<SliderItem>(instance);
        }
    }
}
