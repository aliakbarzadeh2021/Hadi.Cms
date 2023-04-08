using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface ICourseTeacherDto
    {
        Guid Id { get; set; }
        Guid CourseId { get; set; }
        Guid TeacherId { get; set; }
        ICourseDto CourseDto { get; set; }
        ITeacherDto TeacherDto { get; set; }

    }
}
