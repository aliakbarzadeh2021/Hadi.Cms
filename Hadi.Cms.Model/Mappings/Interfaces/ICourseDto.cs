using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface ICourseDto
    {
        Guid Id { get; set; }
        string Title { get; set; }
        string PeriodOfTime { get; set; }
        double Price { get; set; }
        string PriceString { get; set; }
        Guid? AttachmentCoursePreviewVideoId { get; set; }
        string AttachmentCoursePreviewVideoSource { get; set; }
        Guid? CoursePreviewVideoPosterImageId { get; set; }
        string CoursePreviewVideoPosterImageSource { get; set; }
        Guid? AttachmentImageId { get; set; }
        string AttachmentImageSource { get; set; }
        string Description { get; set; }
        bool IsActive { get; set; }
        Guid CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        ICollection<ICourseTeacherDto> CourseTeacherDto { get; set; }
        ICollection<ICourseAttachmentFileDto> CourseAttachmentFileDto { get; set; }
        ICollection<ICourseTagDto> CourseTagDto { get; set; }
    }
}
