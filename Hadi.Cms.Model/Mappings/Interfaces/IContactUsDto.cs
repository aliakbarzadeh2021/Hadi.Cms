using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IContactUsDto
    {
        Guid Id { get; set; }
        DateTime? CreatedWhen { get; set; }
        string UserIp { get; set; }
        string UserName { get; set; }
        string UserEmail { get; set; }
        string UserMobile { get; set; }
        string Subject { get; set; }
        string Text { get; set; }
    }
}
