namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// شهر
    /// </summary>
    public class City
    {
        public City()
        {

        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? ProvinceId { get; set; }
        public string PrePhoneCode { get; set; }
        public virtual Province Province { get; set; }
    }
}
