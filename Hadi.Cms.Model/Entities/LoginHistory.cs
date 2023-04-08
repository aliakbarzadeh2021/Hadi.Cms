using System;

namespace Hadi.Cms.Model.Entities
{
    public class LoginHistory
    {
        public LoginHistory()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool IsLogin { get; set; }
        public User User { get; set; }
    }
}
