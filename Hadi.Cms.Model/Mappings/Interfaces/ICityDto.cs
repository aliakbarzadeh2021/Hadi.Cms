namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface ICityDto
    {
        int Id { get; set; }
        string Name { get; set; }
        int? ProvinceId { get; set; }
        string PrePhoneCode { get; set; }
        IProvinceDto ProvinceDto { get; set; }
    }
}
