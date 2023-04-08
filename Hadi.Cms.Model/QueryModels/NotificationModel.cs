using Hadi.Cms.Language.Resources;
using Hadi.Cms.Model.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Hadi.Cms.Model.QueryModels
{
    public class NotificationModel
    {
        public Guid Id { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NotificationModel_Text")]
        public string Text { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NotificationModel_IsPublic")]
        public bool IsPublic { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NotificationModel_Sender")]
        public User Sender { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NotificationModel_Receiver")]
        public User Receiver { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NotificationModel_ReceiveTime")]
        public DateTime? ReceiveTime { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NotificationModel_SendTime")]
        public DateTime SendTime { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NotificationModel_Status")]
        public bool Status { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NotificationModel_WithNews")]
        public bool WithNews { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "NotificationModel_NewsID")]
        public Guid? NewsId { get; set; }
    }
}
