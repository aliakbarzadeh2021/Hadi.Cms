using System;
using System.Collections.Generic;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class FooterCategoryWithWithLinkDto
    {
        public FooterCategoryWithWithLinkDto()
        {
            FooterLinkDto = new List<FooterLinkDto>();
        }

        public string Title { get; set; }
        public string Link { get; set; }
        public int OrderId { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<FooterLinkDto> FooterLinkDto { get; set; }
    }

    public class FooterLinkDto
    {
        public Guid? ImageAttachmentGuid { get; set; }
        public string ImageAttachmentSource { get; set; }
        public bool HasImage { get; set; }
        public string LinkText { get; set; }
        public string Link { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
