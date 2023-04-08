using System;
using System.Collections.Generic;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class AllCoursesDto
    {
        public AllCoursesDto()
        {
            AllCoursePageDetails = new List<AllCoursePageDetailDto>();
        }

        public IEventDto Event { get; set; }
        public List<AllCoursePageDetailDto> AllCoursePageDetails { get; set; }
    }

    public class AllCoursePageDetailDto
    {
        public AllCoursePageDetailDto()
        {
            Courses = new List<ICourseDto>();
            Teachers = new List<ITeacherDto>();
            Tags = new List<ITagDto>();
            Articles = new List<AllArticleDto>();
            Challenges = new List<IChallengeDto>();
            AttachmentVideos = new List<AttachmentVideoDto>();
        }
        public IServiceDto Service { get; set; }
        public List<ICourseDto> Courses { get; set; }
        public List<ITeacherDto> Teachers { get; set; }
        public List<ITagDto> Tags { get; set; }
        public List<AllArticleDto> Articles { get; set; }
        public List<IChallengeDto> Challenges { get; set; }
        public List<AttachmentVideoDto> AttachmentVideos { get; set; }
    }

    public class AttachmentVideoDto
    {
        public Guid Id { get; set; }
        public string VideoSource { get; set; }
        public string VideoPosterSource { get; set; }
        public string MimeType { get; set; }
    }
}
