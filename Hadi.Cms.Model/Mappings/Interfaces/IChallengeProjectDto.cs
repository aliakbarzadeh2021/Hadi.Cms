using System;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IChallengeProjectDto
    {
        Guid Id { get; set; }
        DateTime CreatedDate { get; set; }
        Guid CreatedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
        Guid? ModifiedBy { get; set; }
        bool IsActive { get; set; }
        bool IsDeleted { get; set; }
        Guid ProjectId { get; set; }
        Guid ChallengeId { get; set; }
        IProjectDto Project { get; set; }
        IChallengeDto Challenge { get; set; }
    }
}
