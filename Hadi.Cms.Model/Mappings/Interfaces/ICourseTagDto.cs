using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface ICourseTagDto
    {
        Guid Id { get; set; }
        Guid CourseId { get; set; }
        Guid TagId { get; set; }
        ICourseDto CourseDto { get; set; }
        ITagDto TagDto { get; set; }
    }
}
