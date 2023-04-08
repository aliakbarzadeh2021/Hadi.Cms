using System;
using System.Linq;
using System.Web.Mvc;
using Hadi.Cms.ApplicationService.Services;

namespace Hadi.Cms.Web.Controllers
{
    public class TechnologiesController : Controller
    {
        private readonly ProjectService _projectService;
        private readonly AttachmentFileService _attachmentFileService;

        public TechnologiesController()
        {
            _projectService = new ProjectService();
            _attachmentFileService = new AttachmentFileService();
        }

        [ChildActionOnly]
        public ActionResult UsedTechnologiesPartial(Guid id)
        {
            var project = _projectService.GetList(p => p.Id == id, null, p => p.ProjectTechnologies,
                p => p.ProjectTechnologies.Select(pt => pt.Technology)).First();

            var technologies = project.ProjectTechnologiesDto.Select(p => p.TechnologyDto).Where(p => !p.IsTool && p.IsActive).ToList();

            foreach (var technology in technologies)
            {
                technology.ImageSource = _attachmentFileService.GetAttachmentSourceValue(technology.ImageGuid);
            }

            return PartialView("_UsedTechnologiesPartial", technologies);
        }

        [ChildActionOnly]
        public ActionResult UsedToolsPartial(Guid id)
        {
            var project = _projectService.GetList(p => p.Id == id, null, p => p.ProjectTechnologies,
                p => p.ProjectTechnologies.Select(pt => pt.Technology)).First();

            var tools = project.ProjectTechnologiesDto.Select(p => p.TechnologyDto).Where(p => p.IsTool && p.IsActive).ToList();

            foreach (var tool in tools)
            {
                tool.ImageSource = _attachmentFileService.GetAttachmentSourceValue(tool.ImageGuid);
            }

            return PartialView("_UsedToolsPartial", tools);
        }
    }
}