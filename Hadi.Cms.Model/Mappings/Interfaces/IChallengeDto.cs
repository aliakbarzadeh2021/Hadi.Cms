using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Hadi.Cms.Model.Entities;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IChallengeDto
    {
        string ProblemDescription { get; set; }
        string ProblemSolvingDescription { get; set; }
        Guid? ImageAttachmentId { get; set; }
        string ImageSource { get; set; }
        Guid? VideoAttachmentId { get; set; }
        string VideoSource { get; set; }
        Guid Id { get; set; }
        DateTime CreatedDate { get; set; }
        Guid CreatedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
        Guid? ModifiedBy { get; set; }
        bool IsActive { get; set; }
        bool IsDeleted { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        string ProjectName { get; set; }
        ICollection<IChallengeProjectDto> ChallengeProjects { get; set; }
    }
}
