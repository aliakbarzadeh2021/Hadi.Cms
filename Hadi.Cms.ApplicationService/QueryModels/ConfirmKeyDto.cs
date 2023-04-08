using Hadi.Cms.Model.Mappings.Interfaces;
using System;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class ConfirmKeyDto : IConfirmKeyDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public bool IsSms { get; set; }
        public bool IsEmail { get; set; }
        public Guid? LinkGuid { get; set; }
        public string SmsKey { get; set; }
        public string UserEmailAddress { get; set; }
        public string UserMobileNumber { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
