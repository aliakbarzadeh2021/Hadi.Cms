using Hadi.Cms.Model.Mappings.Interfaces;
using System;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class EventLogDto : IEventLogDto
    {
        public Guid Id { get; set; }
        public string EventType { get; set; }
        public DateTime EventTime { get; set; }
        public string Source { get; set; }
        public string EventCode { get; set; }
        public Guid? UserId { get; set; }
        public string UserName { get; set; }
        public string IpAddress { get; set; }
        public string EventDescription { get; set; }
        public string EventUrl { get; set; }
        public string EventMachineName { get; set; }
        public string EventUserAgent { get; set; }
        public string EventUrlReferrer { get; set; }
    }
}
