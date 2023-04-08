using Hadi.Cms.Model.Enums;
using System;

namespace Hadi.Cms.Model.Entities
{
    public class UserProfile
    {
        public UserProfile()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string NationalCode { get; set; }
        public Gender? Gender { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public Guid? EducationId { get; set; }
        public int? CityId { get; set; }
        public Guid? ImageAttachmentGuid { get; set; }
        public bool MobileNumberIsValid { get; set; }
        public virtual User User { get; set; }
    }
}
