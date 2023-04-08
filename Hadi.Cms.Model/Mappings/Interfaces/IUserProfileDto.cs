using Hadi.Cms.Model.Enums;
using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IUserProfileDto
    {
        Guid Id { get; set; }
        Guid? UserId { get; set; }

        Guid? ImageAttachmentGuid { get; set; }

        bool MobileNumberIsValid { get; set; }

        //[Display(ResourceType = typeof(Strings), Name = "ProfileModel_User")]
        //  User User { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        DateTime? BirthDate { get; set; }

        string NationalCode { get; set; }

        Gender Gender { get; set; }

        string EmailAddress { get; set; }

        string MobileNumber { get; set; }

        string PhoneNumber { get; set; }

        Guid? EducationId { get; set; }

        int? CityId { get; set; }
        byte[] ImageBinary { get; set; }
        IUserDto UserDto { get; set; }
    }
}
