using System;

namespace Hadi.Cms.Model.Entities
{
    public class PageStatistic : BaseModel
    {
        public PageStatistic()
        {
        }

        public Guid UserId { get; set; }

        /// <summary>
        ///   NewsId or PageId
        /// </summary>
        public Guid? PageId { get; set; }
        public string UserIpAddress { get; set; }
    }
}
