using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IServiceReceiverDto
    {
        Guid Id { get; set; }
        string ReceiverName { get; set; }
        Guid? AttachmentImageId { get; set; }
        string AttachmentImageSource { get; set; }
        bool IsActive { get; set; }
        Guid CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        ICollection<IServiceReceiverServiceDto> ServiceReceiverServiceDto { get; set; }
    }
}
