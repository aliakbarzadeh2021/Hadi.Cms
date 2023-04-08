using Hadi.Cms.Language.Resources;
using Hadi.Cms.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hadi.Cms.Model.QueryModels
{
    public class MailModel
    {
        public Guid Id { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "MailModel_Title")]
        public string Title { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "MailModel_Text")]
        public string Text { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "MailModel_Sender")]
        public User Sender { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "MailModel_Receivers")]
        public List<User> Receivers { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "MailModel_SendTime")]
        public DateTime? SendTime { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "MailModel_Unread")]
        public bool Unread { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "MailModel_WithAttachment")]
        public bool WithAttachment { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "MailModel_Attachments")]
        public List<AttachmentFile> Attachments { get; set; }
    }
}
