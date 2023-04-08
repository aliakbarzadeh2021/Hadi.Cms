using Hadi.Cms.Language.Resources;
using Hadi.Cms.Model.Mappings.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class MailDto : IMailDto
    {
        public Guid Id { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "MailModel_Title")]
        public string Title { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "MailModel_Text")]
        public string Text { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "MailModel_Sender")]
        public IUserDto Sender { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "MailModel_Receivers")]
        public List<IUserDto> Receivers { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "MailModel_SendTime")]
        public DateTime? SendTime { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "MailModel_Unread")]
        public bool Unread { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "MailModel_WithAttachment")]
        public bool WithAttachment { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "MailModel_Attachments")]
        public List<IAttachmentFileDto> Attachments { get; set; }
        public virtual ICollection<IMailAttachmentDto> MailAttachmentsDto { get; set; }

    }
}
