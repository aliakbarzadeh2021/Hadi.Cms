using Hadi.Cms.Model.Mappings.Interfaces;
using System;
using System.Collections.Generic;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class UserDto : IUserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool BuiltIn { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsEnable { get; set; }
        public DateTime CreateDate { get; set; }
        public bool MostChangePassword { get; set; }
        public int LoginRetryCount { get; set; }
        public Guid LanguageId { get; set; }

        public string FullName { get; set; }

        public ILanguageDto LanguageDto { get; set; }
        public List<IUserDto> UserListCollctionDto { get; set; }
        //public User UserListDetail { get; set; }
        public ICollection<ILoginHistoryDto> LoginHistoriesDto { get; set; }
        public ICollection<IUserProfileDto> UserProfilesDto { get; set; }
        public ICollection<IUserRoleDto> UserRolesDto { get; set; }
        public ICollection<IUserContactDto> UserContactsDto { get; set; }
        public ICollection<IPageDto> PagesDto { get; set; }
    }
}
