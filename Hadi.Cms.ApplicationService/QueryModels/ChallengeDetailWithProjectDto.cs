using System.Collections.Generic;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class ChallengeDetailWithProjectDto
    {
        public IChallengeDto ChallengeDto { get; set; }
        public IProjectDto ProjectDto { get; set; }
        public List<IChallengeDto> AnotherChallenges { get; set; }
    }
}
