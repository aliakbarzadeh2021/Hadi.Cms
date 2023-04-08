using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IProvinceDto
    {
        int Id { get; set; }
        string Name { get; set; }
        ICollection<ICityDto> CitiesDto { get; set; }
    }
}
