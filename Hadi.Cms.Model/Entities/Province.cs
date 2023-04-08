using System.Collections.Generic;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// استان
    /// </summary>
    public class Province
    {
        public Province()
        {

        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<City> Cities { get; set; }
    }
}
