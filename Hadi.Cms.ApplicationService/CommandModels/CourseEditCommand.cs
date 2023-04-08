using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ویرایش دوره
    /// </summary>
    public class CourseEditCommand
    {
        public CourseEditCommand()
        {
            CourseVideosId = new List<Guid>();
        }

        public Guid Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "Required")]
        public string PeriodOfTime { get; set; }
        public double Price { get; set; }
        public Guid? AttachmentCoursePreviewVideoId { get; set; }
        public string AttachmentCoursePreviewVideoSource { get; set; }
        public Guid? CoursePreviewVideoPosterImageId { get; set; }
        public string AttachmentCoursePreviewVideoPosterImageSource { get; set; }
        public Guid? AttachmentImageId { get; set; }
        public string AttachmentImageSource { get; set; }
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public List<Guid> CourseTeachersId { get; set; }
        public List<Guid> CourseTagsId { get; set; }
        public List<Guid> CourseVideosId { get; set; }
        public bool IsActive { get; set; }
    }
}
