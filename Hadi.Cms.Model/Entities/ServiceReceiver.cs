using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// دریافت کننده خدمات
    /// </summary>
    public class ServiceReceiver : BaseModel
    {
        public ServiceReceiver()
        {
            ServiceReceiverServices = new HashSet<ServiceReceiverService>();
        }

        public string ReceiverName { get; set; }
        public Guid? AttachmentImageId { get; set; }
        public ICollection<ServiceReceiverService> ServiceReceiverServices { get; set; }
    }
}
