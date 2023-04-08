using System;
using System.Collections.Generic;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ثبت خدمت گیرنده
    /// </summary>
    public class ServiceReceiverCreateCommand
    {
        public string ReceiverName { get; set; }
        public Guid? AttachmentImageId { get; set; }
        public List<Guid> ServicesId { get; set; }
    }
}
