using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ویرایش گیرنده خدمت
    /// </summary>
    public class ServiceReceiverEditCommand
    {
        public Guid Id { get; set; }
        public string ReceiverName { get; set; }
        public string AttachmentImageSource { get; set; }
        public Guid? AttachmentImageId { get; set; }
        public List<Guid> ServicesId { get; set; }
    }
}
