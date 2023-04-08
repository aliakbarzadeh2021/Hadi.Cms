using System;
using System.Linq;
using System.Web.Mvc;
using Hadi.Cms.ApplicationService.QueryModels;
using Hadi.Cms.ApplicationService.Services;

namespace Hadi.Cms.Web.Controllers
{
    /// <summary>
    /// پروژه
    /// </summary>
    public class ProjectsController : Controller
    {
        private readonly ProjectService _projectService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly EmployerService _employerService;

        public ProjectsController()
        {
            _projectService = new ProjectService();
            _attachmentFileService = new AttachmentFileService();
            _employerService = new EmployerService();
        }

        /// <summary>
        /// دریافت لیست پروژه ها
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var projects = _projectService.GetList(p => p.IsActive && !p.IsDeleted,
                o => o.OrderByDescending(p => p.CreatedDate), p => p.ChallengeProjects, p => p.ProjectTags).Take(9).ToList();

            foreach (var project in projects)
                if (project.PreviewImageGuid.HasValue)
                    project.PreviewImageSource = _attachmentFileService.GetAttachmentSourceValue(project.PreviewImageGuid);

            ViewBag.ProjectsCount = _projectService.Count(p => p.IsActive && !p.IsDeleted);
            return View(projects);
        }

        /// <summary>
        /// دریافت لیست پروژه ها -  دکمه پروژه های بیشتر
        /// </summary>
        /// <param name="id"></param>
        /// <param name="serviceId"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public ActionResult GetProjects(int id = 1, Guid? serviceId = null, string search = "")
        {
            var projects = _projectService.Search(serviceId, search);

            projects = projects.OrderByDescending(p => p.CreatedDate).Take(9 * id).ToList();

            foreach (var project in projects)
                if (project.ImageGuid.HasValue)
                    project.Source = _attachmentFileService.GetAttachmentSourceValue(project.ImageGuid);

            return Json(new
            {
                Projects = projects.Select(p => new
                {
                    Title = p.Title,
                    ProjectLink = p.ProjectLink,
                    Source = _attachmentFileService.GetAttachmentSourceValue(p.ImageGuid),
                    ProjectType = p.ProjectType
                }).ToList(),
                ProjectLength = projects.Count,
                Success = true
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// جزئیات یک پروژه
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(Guid id)
        {
            var project = _projectService.GetList(p => p.Id == id && p.IsActive && !p.IsDeleted, null, p => p.ChallengeProjects, p => p.ChallengeProjects.Select(cp => cp.Challenge), p => p.ProjectTags, p => p.ProjectTags.Select(pt => pt.Tag))
                .FirstOrDefault();

            if (project == null)
                return RedirectToAction("Index");

            if (project.ImageGuid.HasValue)
                project.Source = _attachmentFileService.GetAttachmentSourceValue(project.ImageGuid);

            var employer = _employerService.Get(e => e.Id == project.EmployerId && e.IsActive && !e.IsDeleted);
            project.EmployerName = employer?.Name;
            project.EmployerImageSource = _attachmentFileService.GetAttachmentSourceValue(employer?.LogoGuid);
            project.FirstStepImageSource = _attachmentFileService.GetAttachmentSourceValue(project?.FirstStepImage);


            var dto = new ProjectDetailWithChallengeDto
            {
                ProjectDto = project,
                ChallengeDto = project.ChallengeProject.Select(c => c.Challenge).Where(c => c.IsActive && !c.IsDeleted).OrderByDescending(c => c.CreatedDate)
                    .FirstOrDefault()
            };

            if (dto.ChallengeDto != null)
                dto.ChallengeDto.VideoSource =
                    _attachmentFileService.GetAttachmentSourceValue(dto.ChallengeDto.VideoAttachmentId);

            ViewBag.ProjectName = project.Title;

            return View(dto);
        }

        /// <summary>
        /// دریافت لیست پروژه های مشابه
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult RelatedProjectPartial(Guid id)
        {
            var relatedProject = _projectService.GetRelatedProjects(id);
            foreach (var project in relatedProject)
            {
                project.Source = _attachmentFileService.GetAttachmentSourceValue(project.ImageGuid);
            }
            return PartialView("_RelatedProjectParital", relatedProject);
        }

        /// <summary>
        /// تصاویر اسلایدر هر پروژه
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult ProjectSliderImagePartial(Guid id)
        {
            var project = _projectService.GetList(p => p.Id == id, null, p => p.ProjectAttachmentFiles,
                p => p.ProjectAttachmentFiles.Select(pa => pa.AttachmentFile)).First();

            var dto = project.ProjectAttachmentFilesDto.Select(p => new SliderImagePartialDto()
            {
                ImageSource = _attachmentFileService.GetAttachmentSourceValue(p.AttachmentFileId)
            }).ToList();

            return PartialView("_ProjectSliderImagePartial", dto);
        }

        // <summary>
        // جستجوی پروژه ها
        // </summary>
        // <param name = "id" ></ param >
        // < param name="search"></param>
        // <returns></returns>
        public ActionResult ProjectFilter(Guid? id = null, string search = "")
        {
            var dto = _projectService.Search(id, search);

            return Json(new
            {
                Projects = dto.Select(d => new
                {
                    Id = d.Id,
                    Title = d.Title,
                    ProjectType = d.ProjectType,
                    ImageGuid = d.ImageGuid,
                    ImageSource = _attachmentFileService.GetAttachmentSourceValue(d.PreviewImageGuid),
                    CreatedDate = d.CreatedDate,
                }).OrderByDescending(d => d.CreatedDate).ToList(),
            }, JsonRequestBehavior.AllowGet);
        }
    }
}