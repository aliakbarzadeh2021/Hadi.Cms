using System;

namespace Hadi.Cms.Model.Entities
{
    public class ConfirmKey
    {
        /// <summary>
        /// کلید تاییدیه
        /// </summary>
        public ConfirmKey()
        {
            Id = Guid.NewGuid();
        }

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
