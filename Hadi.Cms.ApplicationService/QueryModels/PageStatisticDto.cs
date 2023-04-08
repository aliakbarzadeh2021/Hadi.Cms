using Hadi.Cms.Model.Mappings.Interfaces;
using System;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class PageStatisticDto : IPageStatisticDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Guid? UserId { get; set; }
        public Guid PageId { get; set; }
        public string UserIpAddress { get; set; }
    }
}
