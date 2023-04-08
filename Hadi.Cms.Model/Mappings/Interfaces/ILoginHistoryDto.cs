using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface ILoginHistoryDto
    {
        Guid Id { get; set; }
        Guid UserId { get; set; }
        DateTime? CreateDate { get; set; }
        bool IsLogin { get; set; }
        IUserDto UserDto { get; set; }
    }
}
