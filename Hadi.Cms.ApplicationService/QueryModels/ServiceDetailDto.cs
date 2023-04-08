using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    /// <summary>
    /// دریافت جزئیات خدمت
    /// </summary>
    public class ServiceDetailDto
    {
        public ServiceDetailDto()
        {
            ProjectDto = new List<IProjectDto>();
            TechnologyDto = new List<ITechnologyDto>();

        }
        public IServiceDto ServiceDto { get; set; }
        public List<IProjectDto> ProjectDto { get; set; }
        public List<ITechnologyDto> TechnologyDto { get; set; }
    }
}
