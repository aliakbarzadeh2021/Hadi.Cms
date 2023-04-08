using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface ISettingDto
    {
        Guid Id { get; set; }
        string Key { get; set; }
        string Value { get; set; }
    }
}
