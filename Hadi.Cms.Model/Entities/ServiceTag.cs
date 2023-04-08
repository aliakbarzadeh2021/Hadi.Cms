using System;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// برچسب های سرویس
    /// </summary>
    public class ServiceTag : BaseModel
    {
        public ServiceTag()
        {
            
        }

        public Guid ServiceId { get; set; }
        public Guid TagId { get; set; }
        public Service Service { get; set; }
        public Tag Tag { get; set; }
    }
}
