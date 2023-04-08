using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Infrastructure.Helpers;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Language.Resources;
using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Web.Utilities;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// مدیریت پروژه ها
    /// </summary>
    public class ProjectsController : Controller
    {
        private readonly ProjectService _projectService;
        private readonly EmployerService _employerService;
        private readonly TagService _tagService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly ProjectTagService _projectTagService;
        private readonly EventLoger _eventLoger;
        private readonly ProjectTechnologyService _projectTechnologyService;
        private readonly TechnologyService _technologyService;
        private readonly ProjectAttachmentFileService _projectAttachmentFileService;

        public ProjectsController()
        {
            _projectService = new ProjectService();
            _employerService = new EmployerService();
            _tagService = new TagService();
            _attachmentFileService = new AttachmentFileService();
            _projectTagService = new ProjectTagService();
            _eventLoger = new EventLoger();
            _projectTechnologyService = new ProjectTechnologyService();
            _technologyService = new TechnologyService();
            _projectAttachmentFileService = new ProjectAttachmentFileService();
        }

        /// <summary>
        /// دریافت لیست پروژه ها
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public ActionResult Index(int pageNumber = 1)
        {
            var pageSize = 5;
            var test = SessionData.Current.User.Id;
            var projects = _projectService.GetList(p => !p.IsDeleted && p.CreatedBy == SessionData.Current.User.Id,
                o => o.OrderByDescending(p => p.CreatedDate), p => p.ProjectTags,
                p => p.ProjectTags.Select(pt => pt.Tag), p => p.ChallengeProjects, p => p.ProjectAttachmentFiles, p => p.ProjectAttachmentFiles.Select(pa => pa.AttachmentFile));
            foreach (var project in projects)
            {
                var employerName = _employerService.Get(e => e.Id == project.EmployerId && e.IsActive && !e.IsDeleted)?.Name ?? "";
                project.EmployerName = employerName;
                project.Source = project.ImageGuid.HasValue ? _attachmentFileService.GetAttachmentSourceValue(project.ImageGuid) : "/PhysicalStorage/no_image.png";
            }

            var pagedList = projects.ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;
            TempData["PageNumber"] = pageNumber;
            TempData.Keep("PageNumber");

            return View(pagedList);
        }

        /// <summary>
        /// صفحه ی ثبت پروژه ی جدید
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ViewBag.Employers = _employerService.GetList(e =>
                !e.IsDeleted && e.IsActive && e.CreatedBy == SessionData.Current.User.Id);

            ViewBag.Tags = _tagService.GetList(t =>
                !t.IsDeleted && t.IsActive && t.ParentId != null && t.CreatedBy == SessionData.Current.User.Id);

            ViewBag.Technologies = _technologyService.GetList(t => t.IsActive && !t.IsTool && t.CreatedBy == SessionData.Current.User.Id);

            ViewBag.Tools = _technologyService.GetList(t => t.IsActive && t.IsTool && t.CreatedBy == SessionData.Current.User.Id);

            return View(new ProjectCreateCommand());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectCreateCommand command, HttpPostedFileBase projectPreviewImage, HttpPostedFileBase projectImage,
            HttpPostedFileBase firstStepImage, List<HttpPostedFileBase> sliderImages)
        {
            var existsTitle = _projectService.Any(p => !p.IsDeleted && p.Title == command.Title);
            if (existsTitle)
            {
                ViewBag.Employers = _employerService.GetList(e =>
                !e.IsDeleted && e.IsActive && e.CreatedBy == SessionData.Current.User.Id);

                ViewBag.Tags = _tagService.GetList(t =>
                    !t.IsDeleted && t.IsActive && t.ParentId == null && t.CreatedBy == SessionData.Current.User.Id);

                ViewBag.Technologies = _technologyService.GetList(t => t.IsActive && !t.IsTool && t.CreatedBy == SessionData.Current.User.Id);

                ViewBag.Tools = _technologyService.GetList(t => t.IsActive && t.IsTool && t.CreatedBy == SessionData.Current.User.Id);

                ModelState.AddModelError("Title", Strings.ProjectModel_ExistsTitle);
                return View(command);
            }

            #region Insert image in attachment file

            if (projectImage != null && projectImage.ContentLength > 0)
            {
                byte[] imageData;
                var fileSize = projectImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(projectImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(projectImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(Path.GetExtension(projectImage.FileName));

                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(),
                    System.IO.Path.GetFileName(projectImage.FileName),
                    System.IO.Path.GetExtension(projectImage.FileName), fileSize,
                    MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(projectImage.FileName)),
                    imageWidth, imageHeight, "ProjectImage" + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.ImageGuid = attachmentId;
            }

            if (firstStepImage != null && firstStepImage.ContentLength > 0)
            {
                byte[] imageData;
                var fileSize = firstStepImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(firstStepImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(firstStepImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(Path.GetExtension(firstStepImage.FileName));

                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(),
                    System.IO.Path.GetFileName(firstStepImage.FileName),
                    System.IO.Path.GetExtension(firstStepImage.FileName), fileSize,
                    MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(firstStepImage.FileName)),
                    imageWidth, imageHeight, "FirstStepImage" + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.FirstStepImage = attachmentId;
            }

            if (projectPreviewImage != null && projectPreviewImage.ContentLength > 0)
            {
                byte[] imageData;
                var fileSize = projectPreviewImage.ContentLength;

                using (var binaryReader = new System.IO.BinaryReader(projectPreviewImage.InputStream))
                    imageData = binaryReader.ReadBytes(projectPreviewImage.ContentLength);

                var mimeType = MimeTypeHelper.GetMimeType(Path.GetExtension(projectPreviewImage.FileName));

                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(),
                    Path.GetFileName(projectPreviewImage.FileName),
                    Path.GetExtension(projectPreviewImage.FileName), fileSize,
                    MimeTypeHelper.GetMimeType(Path.GetExtension(projectPreviewImage.FileName)),
                    imageWidth, imageHeight, "PreviewImage" + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.PreviewImageGuid = attachmentId;
            }

            #endregion

            var newProjectId = _projectService.CreateNewProject(command, SessionData.Current.User.Id);

            #region Assign tag to project  

            command.ProjectTagsId?.RemoveAll(p => p == Guid.Empty);

            _projectTagService.AssignTagsToProject(newProjectId, command.ProjectTagsId,
                SessionData.Current.User.Id);

            #endregion

            #region Assign technology and tools to project

            command.TechnologiesId?.RemoveAll(t => t == Guid.Empty);
            command.ToolsId?.RemoveAll(t => t == Guid.Empty);

            _projectTechnologyService.AssignTechnologiesAndToolsToProject(newProjectId, command.TechnologiesId, command.ToolsId,
                SessionData.Current.User.Id);
            #endregion

            #region Insert project slider image to attachment file

            if (sliderImages != null && sliderImages.Count > 0)
            {
                foreach (var sliderImage in sliderImages)
                {
                    if (sliderImage != null && sliderImage.ContentLength > 0)
                    {
                        byte[] imageData;
                        var fileSize = sliderImage.ContentLength;
                        using (var binaryReader = new BinaryReader(sliderImage.InputStream))
                        {
                            imageData = binaryReader.ReadBytes(sliderImage.ContentLength);
                        }

                        var mimeType = MimeTypeHelper.GetMimeType(Path.GetExtension(sliderImage.FileName));

                        var imageWidth = 0;
                        var imageHeight = 0;
                        if (mimeType.Contains("image"))
                        {
                            var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                            imageWidth = img.Width;
                            imageHeight = img.Height;
                        }

                        var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(),
                            Path.GetFileName(sliderImage.FileName),
                            Path.GetExtension(sliderImage.FileName), fileSize,
                            MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(sliderImage.FileName)),
                            imageWidth, imageHeight, "SliderImage" + Guid.NewGuid(), "", imageData, DateTime.Now);

                        command.SliderImageGuid.Add(attachmentId);
                    }
                }
            }
            #endregion

            #region Assign slider image to project
            if (command.SliderImageGuid.Count > 0)
                _projectAttachmentFileService.AssignSliderImageForProject(newProjectId, command.SliderImageGuid, SessionData.Current.User.Id);
            #endregion

            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName,
                "ProjectsController", "Create", "Success Create Project", HttpContext.Request.UserHostAddress,
                HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// صفحه ی ویرایش یک پروژه
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {
            var project = _projectService.GetList(p => p.Id == id && p.CreatedBy == SessionData.Current.User.Id, null, p => p.ChallengeProjects, p => p.ProjectTags,
                p => p.ProjectTags.Select(pt => pt.Tag), p => p.ProjectTechnologies, p => p.ProjectTechnologies.Select(pt => pt.Technology), p => p.ProjectAttachmentFiles, p => p.ProjectAttachmentFiles.Select(pa => pa.AttachmentFile)).FirstOrDefault();
            if (project == null)
                return RedirectToAction("Index", new { pageNumber = int.Parse(TempData["PageNumber"].ToString()) });

            #region Fill tags

            ViewBag.Tags = _tagService.GetList(q =>
                q.IsDeleted == false && q.IsActive && q.ParentId != null && q.CreatedBy == SessionData.Current.User.Id);
            List<string> tagslist = new List<string>();
            foreach (var item in project.ProjectTags)
            {
                tagslist.Add(item.TagId.ToString());
            }

            ViewBag.SelectedTags = string.Join(",", tagslist);

            #endregion

            #region Fill technologies and tools

            #region tech

            ViewBag.Technologies = _technologyService.GetList(t =>
                !t.IsTool && !t.IsDeleted && t.IsActive && t.CreatedBy == SessionData.Current.User.Id);
            List<string> techList = new List<string>();
            foreach (var item in project.ProjectTechnologiesDto.Where(pt => !pt.TechnologyDto.IsTool).ToList())
            {
                techList.Add(item.TechnologyId.ToString());
            }
            ViewBag.SelectedTechnologies = string.Join(",", techList);

            #endregion

            #region tool

            ViewBag.Tools = _technologyService.GetList(t =>
                t.IsTool && !t.IsDeleted && t.IsActive && t.CreatedBy == SessionData.Current.User.Id);
            List<string> toolList = new List<string>();
            foreach (var item in project.ProjectTechnologiesDto.Where(pt => pt.TechnologyDto.IsTool).ToList())
            {
                toolList.Add(item.TechnologyId.ToString());
            }
            ViewBag.SelectedTools = string.Join(",", toolList);

            #endregion

            #endregion

            ViewBag.Employers = _employerService.GetList(e =>
                !e.IsDeleted && e.IsActive && e.CreatedBy == SessionData.Current.User.Id);

            foreach (var item in project.ProjectAttachmentFilesDto.Select(pa => pa.AttachmentFileDto).ToList())
                item.Source = _attachmentFileService.GetAttachmentSourceValue(item.Id);

            return View(new ProjectEditCommand
            {
                Id = project.Id,
                EmployerId = project.EmployerId,
                PreviewImageGuid = project.PreviewImageGuid,
                PreviewImageSource = _attachmentFileService.GetAttachmentSourceValue(project.PreviewImageGuid),
                ImageGuid = project.ImageGuid,
                ImageSource = _attachmentFileService.GetAttachmentSourceValue(project.ImageGuid),
                ProjectLink = project.ProjectLink,
                ProjectLinkText = project.ProjectLinkText,
                Title = project.Title,
                ProjectType = project.ProjectType,
                Description = project.Description,
                IsActive = project.IsActive,
                FirstStepImage = project.FirstStepImage,
                FirstStepImageSource = _attachmentFileService.GetAttachmentSourceValue(project.FirstStepImage),
                FirstStepDescription = project.FirstStepDescription,
                SecondStepDescription = project.SecondStepDescription,
                FinalStepDescription = project.FinalStepDescription,
                AttachmentFiles = project.ProjectAttachmentFilesDto.Select(pa => pa.AttachmentFileDto).ToList()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectEditCommand command, HttpPostedFileBase projectPreviewImage, HttpPostedFileBase projectImage,
            HttpPostedFileBase firstStepImage, List<HttpPostedFileBase> sliderImages)
        {
            var project = _projectService.Get(p => p.Id == command.Id).MapToEntity();

            #region Insert image in attachment file

            if (projectImage != null && projectImage.ContentLength > 0)
            {
                if (command.ImageGuid != null && command.ImageGuid != Guid.Empty)
                    _attachmentFileService.RemoveAttachment(command.ImageGuid.Value);

                byte[] imageData;
                var fileSize = projectImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(projectImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(projectImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(Path.GetExtension(projectImage.FileName));

                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(),
                    Path.GetFileName(projectImage.FileName),
                    Path.GetExtension(projectImage.FileName), fileSize,
                    MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(projectImage.FileName)),
                    imageWidth, imageHeight, "ProjectImage" + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.ImageGuid = attachmentId;
            }

            if (firstStepImage != null && firstStepImage.ContentLength > 0)
            {
                if (command.FirstStepImage != null && command.FirstStepImage != Guid.Empty)
                    _attachmentFileService.RemoveAttachment(command.FirstStepImage.Value);

                byte[] imageData;
                var fileSize = firstStepImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(firstStepImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(firstStepImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(Path.GetExtension(firstStepImage.FileName));

                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(),
                    Path.GetFileName(firstStepImage.FileName),
                    attachmentExtension: System.IO.Path.GetExtension(firstStepImage.FileName), fileSize,
                    MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(firstStepImage.FileName)),
                    imageWidth, imageHeight, "FirstStepImage" + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.FirstStepImage = attachmentId;
            }

            if (projectPreviewImage != null && projectPreviewImage.ContentLength > 0)
            {
                if (project.PreviewImageGuid.HasValue)
                    _attachmentFileService.RemoveAttachment(project.PreviewImageGuid.Value);

                byte[] imageData;
                var fileSize = projectPreviewImage.ContentLength;

                using (var binaryReader = new BinaryReader(projectPreviewImage.InputStream))
                    imageData = binaryReader.ReadBytes(projectPreviewImage.ContentLength);

                var mimeType = MimeTypeHelper.GetMimeType(Path.GetExtension(projectPreviewImage.FileName));

                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(),
                    Path.GetFileName(projectPreviewImage.FileName),
                    Path.GetExtension(projectPreviewImage.FileName), fileSize,
                    MimeTypeHelper.GetMimeType(Path.GetExtension(projectPreviewImage.FileName)),
                    imageWidth, imageHeight, "PreviewImage" + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.PreviewImageGuid = attachmentId;
            }

            #endregion

            var existsTitle = _projectService.Any(p => !p.IsDeleted && p.Title == command.Title && p.Id != command.Id);
            if (existsTitle)
            {
                ViewBag.Employers = _employerService.GetList(e =>
                !e.IsDeleted && e.IsActive && e.CreatedBy == SessionData.Current.User.Id);

                ViewBag.Tags = _tagService.GetList(t =>
                    !t.IsDeleted && t.IsActive && t.ParentId == null && t.CreatedBy == SessionData.Current.User.Id);

                ViewBag.Technologies = _technologyService.GetList(t => t.IsActive && !t.IsTool && t.CreatedBy == SessionData.Current.User.Id);

                ViewBag.Tools = _technologyService.GetList(t => t.IsActive && t.IsTool && t.CreatedBy == SessionData.Current.User.Id);

                ModelState.AddModelError("Title", Strings.ProjectModel_ExistsTitle);
                return View(command);
            }

            _projectService.UpdateProject(project, command, SessionData.Current.User.Id);

            #region Assign tag to project
            command.ProjectTagsId?.RemoveAll(pt => pt == Guid.Empty);
            _projectTagService.AssignTagsToProject(project.Id, command.ProjectTagsId, SessionData.Current.User.Id);
            #endregion

            #region Assign technology and tools to project

            _projectTechnologyService.AssignTechnologiesAndToolsToProject(project.Id, command.TechnologiesId,
                command.ToolsId, SessionData.Current.User.Id);
            #endregion

            #region Insert project slider imgae to attachment file

            if (sliderImages != null && sliderImages.Count > 0)
            {
                foreach (var sliderImage in sliderImages)
                {
                    if (sliderImage != null && sliderImage.ContentLength > 0)
                    {
                        byte[] imageData;
                        var fileSize = sliderImage.ContentLength;
                        using (var binaryReader = new System.IO.BinaryReader(sliderImage.InputStream))
                        {
                            imageData = binaryReader.ReadBytes(sliderImage.ContentLength);
                        }

                        var mimeType = MimeTypeHelper.GetMimeType(Path.GetExtension(sliderImage.FileName));

                        var imageWidth = 0;
                        var imageHeight = 0;
                        if (mimeType.Contains("image"))
                        {
                            var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                            imageWidth = img.Width;
                            imageHeight = img.Height;
                        }

                        var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(),
                            System.IO.Path.GetFileName(sliderImage.FileName),
                            System.IO.Path.GetExtension(sliderImage.FileName), fileSize,
                            MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(sliderImage.FileName)),
                            imageWidth, imageHeight, "SliderImage" + Guid.NewGuid(), "", imageData, DateTime.Now);

                        command.SliderImageGuid.Add(attachmentId);
                    }
                }
            }
            #endregion

            #region Assign slider image to project
            if (command.SliderImageGuid.Count > 0)
                _projectAttachmentFileService.AssignSliderImageForProject(project.Id, command.SliderImageGuid, SessionData.Current.User.Id);
            #endregion

            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "ProjectsController", "Edit", "Success Edit Project", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { pageNumber = int.Parse(TempData["PageNumber"].ToString()) });
        }

        /// <summary>
        /// تغییر وضعیت پروژه
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ChangeStatus(Guid id)
        {
            var project = _projectService.Get(p => p.Id == id).MapToEntity();
            if (project == null)
                return RedirectToAction("Index", new { pageNumber = int.Parse(TempData["PageNumber"].ToString()) });

            project.IsActive = !project.IsActive;
            _projectService.Update(project);
            _projectService.Save();
            return RedirectToAction("Index", new { pageNumber = int.Parse(TempData["PageNumber"].ToString()) });
        }
        /// <summary>
        /// حذف پروژه
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var project = _projectService.Get(p => p.Id == id);

                project.IsActive = false;
                project.IsDeleted = true;
                _projectService.Update(project.MapToEntity());
                _projectService.Save();
                return Json(new
                {
                    Message = Strings.Project_Delete_Successfully,
                    Success = Strings.Success,
                    Type = "success"
                });
            }
            catch
            {
                return Json(new
                {
                    Message = Strings.Global_SystemError,
                    Success = Strings.Error,
                    Type = "error"
                });
            }
        }
        /// <summary>
        /// حذف عکس های اسلایدر پروژه
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="attachmentFileId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteSliderImage(Guid attachmentFileId, Guid projectId)
        {
            try
            {
                var projectAttachmentFile = _projectAttachmentFileService.Get(p => p.AttachmentFileId == attachmentFileId && p.ProjectId == projectId);

                if (projectAttachmentFile == null)
                {
                    return Json(new
                    {
                        Message = Strings.ProjectModel_ProjectAttachmentFieNotFound,
                        Success = Strings.Error,
                        Type = "error"
                    }, JsonRequestBehavior.AllowGet);
                }

                _projectAttachmentFileService.Delete(projectAttachmentFile.Id);
                _projectAttachmentFileService.Save();

                return Json(new
                {
                    Message = Strings.ProjectModel_ProjectAttachmentFileHasBeenDeleted,
                    Success = Strings.Success,
                    Type = "success"
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Message = Strings.Global_SystemError,
                    Success = Strings.Error,
                    Type = "error"
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}