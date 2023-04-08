using System;

namespace Hadi.Cms.Model.Entities
{
    public class IpRange
    {
        public IpRange()
        {
            Id = Guid.NewGuid();
            CreateDate = DateTime.Now;
        }

        public Guid Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Lower { get; set; }
        public string Upper { get; set; }
        public bool IsActive { get; set; }
    }
}
