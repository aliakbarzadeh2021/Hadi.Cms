using Hadi.Cms.Model.Mappings.Interfaces;
using System.Collections.Generic;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class ProvinceDto : IProvinceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ICityDto> CitiesDto { get; set; }
    }
}
