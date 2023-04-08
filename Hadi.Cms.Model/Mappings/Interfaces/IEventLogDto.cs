using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IEventLogDto
    {
        Guid Id { get; set; }
        string EventType { get; set; }
        DateTime EventTime { get; set; }
        string Source { get; set; }
        string EventCode { get; set; }
        Guid? UserId { get; set; }
        string UserName { get; set; }
        string IpAddress { get; set; }
        string EventDescription { get; set; }
        string EventUrl { get; set; }
        string EventMachineName { get; set; }
        string EventUserAgent { get; set; }
        string EventUrlReferrer { get; set; }
    }
}
