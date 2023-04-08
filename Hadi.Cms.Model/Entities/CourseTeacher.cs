using System;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// مدرس دوره
    /// </summary>
    public class CourseTeacher : BaseModel
    {
        public CourseTeacher()
        {

        }

        /// <summary>
        /// شناسه دوره
        /// </summary>
        public Guid CourseId { get; set; }
        /// <summary>
        /// شناسه مدرس
        /// </summary>
        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public Course Course { get; set; }
    }
}
