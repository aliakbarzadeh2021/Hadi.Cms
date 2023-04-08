using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hadi.Cms.Model.Entities
{
    public class UserRole
    {
        public UserRole()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
