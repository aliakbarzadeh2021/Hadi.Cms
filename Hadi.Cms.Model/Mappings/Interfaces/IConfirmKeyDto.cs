using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IConfirmKeyDto
    {
        Guid Id { get; set; }
        Guid? UserId { get; set; }
        bool IsSms { get; set; }
        bool IsEmail { get; set; }
        Guid? LinkGuid { get; set; }
        string SmsKey { get; set; }
        string UserEmailAddress { get; set; }
        string UserMobileNumber { get; set; }
        DateTime? CreateDate { get; set; }
        DateTime? ExpireDate { get; set; }
    }
}
