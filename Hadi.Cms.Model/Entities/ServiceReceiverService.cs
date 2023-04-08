using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// خدمات خدمت گیرنده
    /// </summary>
    public class ServiceReceiverService : BaseModel
    {
        public ServiceReceiverService()
        {
            
        }

        public Guid ServiceReceiverId { get; set; }
        public Guid ServiceId { get; set; }
        public ServiceReceiver ServiceReceiver { get; set; }
        public Service Service { get; set; }
    }
}
