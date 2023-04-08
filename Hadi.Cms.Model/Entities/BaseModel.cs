using System;
using System.ComponentModel.DataAnnotations;

namespace Hadi.Cms.Model.Entities
{
    public class BaseModel
    {
        public BaseModel()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.Now;
            IsActive = true;
            IsDeleted = false;
        }

        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }     
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
