using System;
using System.Collections.Generic;
using System.Web;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    public class AttachmentFileCreateCommand
    {
        public HttpPostedFile File { get; set; }
        public List<Guid> TagsId { get; set; }
    }
}
