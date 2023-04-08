using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    public class ProjectHomePageSectionDto
    {
        public ProjectHomePageSectionDto()
        {
            AnotherEmployerProject = new List<IProjectDto>();
        }
        public IProjectDto Project { get; set; }
        public List<IProjectDto> AnotherEmployerProject { get; set; }
    }
}
