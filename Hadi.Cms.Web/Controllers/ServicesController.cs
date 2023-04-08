using System;
using System.Collections.Generic;
using System.Linq;
using Hadi.Cms.ApplicationService.Services;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Hadi.Cms.ApplicationService.QueryModels;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Model.Mappings.Mappers;

namespace Hadi.Cms.Web.Controllers
{
    /// <summary>
    /// سرویس
    /// </summary>
    public class ServicesController : Controller
    {
        private readonly ServiceService _serviceService;
        private readonly ProjectService _projectService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly TagService _tagService;
        private readonly FormService _formService;
        private readonly FormFieldService _formFieldService;

        public ServicesController()
        {
            _serviceService = new ServiceService();
            _projectService = new ProjectService();
            _attachmentFileService = new AttachmentFileService();
            _tagService = new TagService();
            _formService = new FormService();
            _formFieldService = new FormFieldService();
        }

        public ActionResult ServicePartial()
        {
            var services = _serviceService.GetList(s => s.IsActive && !s.IsDeleted);
            return PartialView("_ServicePartial", services);
        }

        /// <summary>
        /// جزئیات سرویس
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(Guid id)
        {
            var service = _serviceService.GetList(s => s.IsActive && !s.IsDeleted && s.Id == id, null,
                s => s.ServiceTags, s => s.ServiceTags.Select(st => st.Tag), s => s.ServiceComments, s => s.ServiceReceiverServices, s => s.ServiceReceiverServices.Select(sr => sr.ServiceReceiver)).FirstOrDefault();
            if (service == null)
                return HttpNotFound("Service not found .");

            var relatedProjects = new List<IProjectDto>();
            var technologies = new List<ITechnologyDto>();

            var serviceTags = service.ServiceTagDto.Select(st => st.TagDto).Where(t => t.IsActive).ToList();
            foreach (var serviceTag in serviceTags)
            {
                var parentTag = _tagService.Get(t => t.IsActive && !t.IsDeleted && t.Id == serviceTag.ParentId);
                if(parentTag != null)
                {
                    var childTags = _tagService.GetList(t => t.ParentId == parentTag.Id);

                    foreach (var childTag in childTags)
                    {
                        var projects = _projectService.GetList(p =>
                            p.IsActive && !p.IsDeleted && p.ProjectTags.Any(pt => pt.TagId == childTag.Id), null,
                            p => p.ProjectTags, p => p.ProjectTags.Select(pt => pt.Tag),
                            p => p.ProjectTechnologies,
                            p => p.ProjectTechnologies.Select(pt => pt.Technology));

                        relatedProjects.AddRange(projects);
                    }
                }
            }
            relatedProjects = relatedProjects.DistinctBy(r => r.Id).OrderByDescending(r => r.CreatedDate).Take(4).ToList();


            foreach (var relatedProject in relatedProjects)
            {
                relatedProject.Source = _attachmentFileService.GetAttachmentSourceValue(relatedProject.ImageGuid);

                var techs = relatedProject.ProjectTechnologiesDto.Select(p => p.TechnologyDto).Where(t => t.IsActive && !t.IsDeleted).ToList();
                technologies.AddRange(techs);
            }

            technologies = technologies.DistinctBy(t => t.Id).ToList();
            foreach (var technology in technologies)
                technology.ImageSource = _attachmentFileService.GetAttachmentSourceValue(technology.ImageGuid);

            #region Image source
            service.AttachmentImageSource = _attachmentFileService.GetAttachmentSourceValue(service.AttachmentImageId);

            service.SectionOneImageSource =
                _attachmentFileService.GetAttachmentSourceValue(service.SectionOneImageGuid);

            service.SectionOneThumbnailImageSource =
                _attachmentFileService.GetAttachmentSourceValue(service.SectionOneThumbnailImageGuid);

            service.SectionTwoImageSource =
                _attachmentFileService.GetAttachmentSourceValue(service.SectionTwoImageGuid);

            service.SectionTwoThumbnailImageSource =
                _attachmentFileService.GetAttachmentSourceValue(service.SectionTwoThumbnailImageGuid);

            service.SectionThreeImageSource =
                _attachmentFileService.GetAttachmentSourceValue(service.SectionThreeImageGuid);

            service.SectionThreeThumbnailImageSource =
                _attachmentFileService.GetAttachmentSourceValue(service.SectionThreeThumbnailImageGuid);

            service.SectionFourThumbnailImageSource =
                _attachmentFileService.GetAttachmentSourceValue(service.SectionFourThumbnailImageGuid);

            service.SectionFourImageSource =
                _attachmentFileService.GetAttachmentSourceValue(service.SectionFourImageGuid);

            foreach (var serviceReceiverDto in service.ServiceReceiverServiceDto.Select(s => s.ServiceReceiverDto).Where(s => s.IsActive).ToList())
            {
                serviceReceiverDto.AttachmentImageSource =
                    _attachmentFileService.GetAttachmentSourceValue(serviceReceiverDto.AttachmentImageId);
            }

            foreach (var serviceComment in service.ServiceCommentDto)
            {
                serviceComment.AttachmentImageSource =
                    _attachmentFileService.GetAttachmentSourceValue(serviceComment.AttachmentImageId);
            }
            #endregion

            service.Title = service.Title?.Replace("<br/>", " ");

            var dto = new ServiceDetailDto()
            {
                ServiceDto = service,
                ProjectDto = relatedProjects,
                TechnologyDto = technologies
            };

            #region Service form

            var form = _formService.Get(f => f.Name.ToLower().Contains("service"));

            ViewBag.FormDataSource = form != null ? form.FormDataSource : "";

            #endregion

            var serviceMapedToEntity = _serviceService.Get(s => s.Id == id).MapToEntity();
            serviceMapedToEntity.ReviewCount += 1;
            _serviceService.Update(serviceMapedToEntity);
            _serviceService.Save();

            return View(dto);
        }
    }
}