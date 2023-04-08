using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Web.Controllers
{
    //home controller
    public class HomeController : Controller
    {
        private readonly DepartmentService _departmentService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly ProjectService _projectService;
        private readonly ServiceService _serviceService;
        private readonly EmployerService _employerService;


        public HomeController()
        {
            _departmentService = new DepartmentService();
            _attachmentFileService = new AttachmentFileService();
            _projectService = new ProjectService();
            _serviceService = new ServiceService();
            _employerService = new EmployerService();
        }
        public ActionResult Index()
        {
            var employers = _employerService.GetList(e => e.IsActive && !e.IsDeleted && e.Projects.Any(), e => e.OrderByDescending(o => o.CreatedDate), e => e.Projects);

            foreach (var employer in employers)
                employer.LogoSource = _attachmentFileService.GetAttachmentSourceValue(employer.LogoGuid);

            ViewBag.Employers = employers;
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View("Contact");
        }

        public ActionResult Redkala()
        {
            return View();
        }

        public ActionResult Navaar()
        {
            return View();
        }

        public ActionResult Career()
        {
            return View();
        }

        public ActionResult DepartmentPartial(bool mobileVersion = false)
        {
            var departments = _departmentService.GetList(d => d.IsActive && !d.IsDeleted);
            foreach (var department in departments)
            {
                department.Source = _attachmentFileService.GetAttachmentSourceValue(department.AttachmentImageId);
            }
            departments = departments.OrderByDescending(d => d.CreatedDate).Take(3).ToList();

            ViewBag.MobileVersion = mobileVersion;

            return PartialView("_DepartmentPartial", departments);
        }

        /// <summary>
        /// سکشن پروژه ها در صفحه ی اصلی سایت
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ProjectsPartial(Guid id)
        {
            var projects = _projectService.GetList(p => p.IsActive && !p.IsDeleted && p.EmployerId == id, null,
                p => p.ProjectTags, p => p.ProjectTags.Select(pt => pt.Tag));

            var project = projects.OrderByDescending(p => p.CreatedDate).FirstOrDefault(p => p.EmployerId == id);

            projects.Remove(project);

            var departments = new List<IDepartmentDto>();

            if (project != null)
            {
                if (project.PreviewImageGuid.HasValue)
                    project.PreviewImageSource = _attachmentFileService.GetAttachmentSourceValue(project.PreviewImageGuid);

                var projectTags = project.ProjectTags.Select(pt => pt.TagDto).ToList();

                foreach (var projectTag in projectTags)
                {
                    var services = _serviceService.GetList(s => s.ServiceTags.Any(st => st.TagId == projectTag.Id), null,
                        s => s.DepartmentServices, s => s.DepartmentServices.Select(ds => ds.Department));

                    var selectedDepartment = services.SelectMany(s => s.DepartmentServicesDto.Select(ds => ds.DepartmentDto)).ToList();

                    departments.AddRange(selectedDepartment);
                }
            }

            return Json(new
            {
                Id = project?.Id,
                Title = project?.Title,
                Description = project?.Description,
                ImageSource = project?.PreviewImageSource,
                ProjectDepartments = departments.DistinctBy(d => d.Id).Select(d => new
                {
                    Id = d.Id,
                    Title = d.Title,
                    Color = d.Color,
                    AttachmentImageSource = _attachmentFileService.GetAttachmentSourceValue(d.AttachmentImageId),
                }).ToList(),
                RelatedProjects = projects.OrderByDescending(p => p.CreatedDate).Take(4).Select(p => new
                {
                    Id = p.Id,
                    Title = p.Title
                }).ToList()
            }, JsonRequestBehavior.AllowGet);
        }

    }
}