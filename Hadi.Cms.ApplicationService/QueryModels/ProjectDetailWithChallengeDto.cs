using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class ProjectDetailWithChallengeDto
    {
        public IProjectDto ProjectDto { get; set; }
        public IChallengeDto ChallengeDto { get; set; }
    }
}
