using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IEventDto
    {
        Guid Id { get; set; }
        string Title { get; set; }
        string Link { get; set; }
        Guid? AttachmentImageId { get; set; }
        string AttachmentImageSource { get; set; }
        bool IsActive { get; set; }
        Guid CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
    }
}
