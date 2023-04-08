using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class CityDto : ICityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ProvinceId { get; set; }
        public string PrePhoneCode { get; set; }
        public IProvinceDto ProvinceDto { get; set; }
    }
}
