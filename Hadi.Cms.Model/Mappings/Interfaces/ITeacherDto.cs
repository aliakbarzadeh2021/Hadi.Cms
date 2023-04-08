using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface ITeacherDto
    {
        Guid Id { get; set; }
        DateTime CreatedDate { get; set; }
        Guid CreatedBy { get; set; }
        bool IsActive { get; set; }
        string FullName { get; set; }
        Guid? AttachmentImageId { get; set; }
        string AttachmentImageSource { get; set; }
        string SocialNetworkName { get; set; }
        string SocialNetworkLink { get; set; }
        Guid? SocialNetworkImageGuid { get; set; }
        string SocialNetworkImageSource { get; set; }

        ICollection<ICourseTeacherDto> CourseTeacherDto { get; set; }
    }
}
