using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IUserContactDto
    {
        Guid Id { get; set; }
        DateTime? CreateDate { get; set; }
        Guid UserId { get; set; }
        Guid ContactUserId { get; set; }
        IUserDto SelfUserDto { get; set; }
        IUserDto ContactUserDto { get; set; }
    }
}
