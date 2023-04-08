using System;

namespace Hadi.Cms.Model.Entities
{
    public class Setting
    {
        public Setting()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
