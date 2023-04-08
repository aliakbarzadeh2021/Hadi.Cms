using Hadi.Cms.Model.Mappings.Interfaces;
using System;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class UserContactDto : IUserContactDto
    {
        public Guid Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid UserId { get; set; }
        public Guid ContactUserId { get; set; }
        public IUserDto SelfUserDto { get; set; }
        public IUserDto ContactUserDto { get; set; }
    }
}
