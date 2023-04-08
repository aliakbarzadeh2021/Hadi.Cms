using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IProjectDto
    {
        Guid Id { get; set; }
        Guid EmployerId { get; set; }
        string EmployerImageSource { get; set; }
        string EmployerName { get; set; }
        string Title { get; set; }
        string ProjectType { get; set; }
         Guid? PreviewImageGuid { get; set; }
         string PreviewImageSource { get; set; }
        Guid? ImageGuid { get; set; }
        Guid? FirstStepImage { get; set; }
        string FirstStepDescription { get; set; }
        string SecondStepDescription { get; set; }
        string FinalStepDescription { get; set; }
        string FirstStepImageSource { get; set; }
        string ProjectLinkText { get; set; }
        string Source { get; set; }
        string ProjectLink { get; set; }
        string Description { get; set; }
        Guid CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        bool IsActive { get; set; }
        bool IsDeleted { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        ICollection<IProjectTagDto> ProjectTags { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        ICollection<IChallengeProjectDto> ChallengeProject { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        ICollection<IProjectTechnologyDto> ProjectTechnologiesDto { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        ICollection<IProjectAttachmentFileDto> ProjectAttachmentFilesDto { get; set; }
    }
}
