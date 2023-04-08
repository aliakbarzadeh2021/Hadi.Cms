using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IUserRoleDto
    {
        Guid Id { get; set; }
        Guid UserId { get; set; }
        Guid RoleId { get; set; }
        IRoleDto RoleDto { get; set; }
        IUserDto UserDto { get; set; }
    }
}
