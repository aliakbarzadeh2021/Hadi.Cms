using Hadi.Cms.Model.Mappings.Interfaces;
using System;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class LoginHistoryDto : ILoginHistoryDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool IsLogin { get; set; }
        public IUserDto UserDto { get; set; }
    }
}
