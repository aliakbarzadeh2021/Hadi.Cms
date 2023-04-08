
using System;

namespace Hadi.Cms.Model.Entities
{
    public class UserContact
    {
        public UserContact()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid UserId { get; set; }
        public Guid ContactUserId { get; set; }
        public User SelfUser { get; set; }
        public User ContactUser { get; set; }
    }
}
