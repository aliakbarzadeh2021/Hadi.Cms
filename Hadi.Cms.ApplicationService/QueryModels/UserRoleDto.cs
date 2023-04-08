using Hadi.Cms.Model.Mappings.Interfaces;
using System;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class UserRoleDto : IUserRoleDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public IRoleDto RoleDto { get; set; }
        public IUserDto UserDto { get; set; }
    }
}
