using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IServiceReceiverServiceDto
    {
        Guid Id { get; set; }
        Guid ServiceReceiverId { get; set; }
        Guid ServiceId { get; set; }
        IServiceDto ServiceDto { get; set; }
        IServiceReceiverDto ServiceReceiverDto { get; set; }
    }
}
