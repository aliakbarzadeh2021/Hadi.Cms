
using System;

namespace Hadi.Cms.Model.Entities
{
    public class IpBanned
    {
        public IpBanned()
        {
            Id = Guid.NewGuid();
            CreateDate = DateTime.Now;
        }

        public Guid Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string IpAddressBanReason { get; set; }
        public string IpAddress { get; set; }
        public bool IsActive { get; set; }
    }
}
