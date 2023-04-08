using System;
using System.ComponentModel.DataAnnotations;
using Hadi.Cms.Language.Resources;
using Hadi.Cms.Model.Entities;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    public class NotificationCommand
    {

        [Display(ResourceType = typeof(Strings), Name = "NotificationModel_Text")]
        [Required(AllowEmptyStrings = false , ErrorMessageResourceType = typeof(Strings),ErrorMessageResourceName = "Required")]
        public string Text { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NotificationModel_IsPublic")]
        public bool IsPublic { get; set; } = false;

        [Display(ResourceType = typeof(Strings), Name = "NotificationModel_Sender")]
        public User Sender { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NotificationModel_Receiver")]
        public Guid? ReceiverId { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NotificationModel_ReceiveTime")]
        public DateTime? ReceiveTime { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NotificationModel_SendTime")]
        public DateTime SendTime { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NotificationModel_WithNews")]
        public bool WithNews { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NotificationModel_NewsID")]
        public Guid? NewsId { get; set; }
    }
}
