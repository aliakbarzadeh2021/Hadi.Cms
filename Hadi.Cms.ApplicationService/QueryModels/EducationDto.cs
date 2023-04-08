using Hadi.Cms.Model.Mappings.Interfaces;
using System;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class EducationDto : IEducationDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}
