using Hadi.Cms.Language.Resources;
using Hadi.Cms.Model.Enums;
using Hadi.Cms.Model.Mappings.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class UserProfileDto : IUserProfileDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }

        public Guid? ImageAttachmentGuid { get; set; }

        public bool MobileNumberIsValid { get; set; }

        //[Display(ResourceType = typeof(Strings), Name = "ProfileModel_User")]
        //public User User { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "ProfileModel_User_FirstName")]
        public string FirstName { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "ProfileModel_User_LastName")]
        public string LastName { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "ProfileModel_BirthDate")]
        public DateTime? BirthDate { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "ProfileModel_NationalCode")]
        public string NationalCode { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "ProfileModel_Gender")]
        public Gender Gender { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "ProfileModel_EmailAddress")]
        public string EmailAddress { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "ProfileModel_MobileNumber")]
        public string MobileNumber { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "ProfileModel_PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "ProfileModel_EducationId")]
        public Guid? EducationId { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "ProfileModel_CityId")]
        public int? CityId { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "ProfileModel_ImageBinary")]
        public byte[] ImageBinary { get; set; }
        [Display(ResourceType = typeof(Strings),Name = "ProfileModel_LinkedInAddress")]
        public string LinkedInAddress { get; set; }
        [Display(ResourceType = typeof(Strings), Name = "ProfileModel_InstagramAddress")]
        public string InstagramAddress { get; set; }
        [Display(ResourceType = typeof(Strings), Name = "ProfileModel_TelegramAddress")]
        public string TelegramAddress { get; set; }
        public IUserDto UserDto { get; set; }
    }
}
