using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IUserDto
    {
        Guid Id { get; set; }
        string UserName { get; set; }
        string PasswordHash { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        bool BuiltIn { get; set; }
        bool IsDeleted { get; set; }
        bool IsEnable { get; set; }
        DateTime CreateDate { get; set; }
        bool MostChangePassword { get; set; }
        int LoginRetryCount { get; set; }
        Guid LanguageId { get; set; }
        string FullName { get; set; }

        ILanguageDto LanguageDto { get; set; }
        List<IUserDto> UserListCollctionDto { get; set; }
        // User UserListDetail { get; set; }
        ICollection<ILoginHistoryDto> LoginHistoriesDto { get; set; }
        ICollection<IUserProfileDto> UserProfilesDto { get; set; }
        ICollection<IUserRoleDto> UserRolesDto { get; set; }
        ICollection<IUserContactDto> UserContactsDto { get; set; }
        ICollection<IPageDto> PagesDto { get; set; }
    }
}

