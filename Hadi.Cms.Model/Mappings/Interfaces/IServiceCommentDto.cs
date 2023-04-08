using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IServiceCommentDto
    {
        Guid Id { get; set; }
        Guid ServiceId { get; set; }
        string Text { get; set; }
        string PersonFullName { get; set; }
        string PersonRoleTitle { get; set; }
        string AttachmentImageSource { get; set; }
        Guid? AttachmentImageId { get; set; }
        Guid CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        IServiceDto ServiceDto { get; set; }
    }
}
