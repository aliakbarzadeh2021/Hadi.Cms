using System;
using System.Collections.Generic;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// مدرسین
    /// </summary>
    public class Teacher : BaseModel
    {
        public Teacher()
        {
            CourseTeachers = new HashSet<CourseTeacher>();
        }
        /// <summary>
        /// نام کامل
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// شناسه عکس مدرس
        /// </summary>
        public Guid? AttachmentImageId { get; set; }

        public string SocialNetworkName { get; set; }
        public string SocialNetworkLink { get; set; }
        public Guid? SocialNetworkImageGuid { get; set; }

        public ICollection<CourseTeacher> CourseTeachers { get; set; }
    }
}
