using System;
using System.Collections.Generic;
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
    /// مدیریت چالش ها
    /// </summary>
    public class ChallengeController : Controller
    {
        private readonly ChallengeService _challengeService;
        private readonly ChallengeProjectService _challengeProjectService;
        private readonly ProjectService _projectService;
        private readonly EventLoger _eventLoger;
        private readonly AttachmentFileService _attachmentFileService;

        public ChallengeController()
        {
            _challengeService = new ChallengeService();
            _challengeProjectService = new ChallengeProjectService();
            _projectService = new ProjectService();
            _eventLoger = new EventLoger();
            _attachmentFileService = new AttachmentFileService();
        }

        /// <summary>
        /// دریافت لیست چالش ها
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public ActionResult Index(int pageNumber = 1)
        {
            var pageSize = 5;
            var challenges = _challengeService.GetList(c => !c.IsDeleted && c.CreatedBy == SessionData.Current.User.Id, o => o.OrderByDescending(c => c.CreatedDate), c => c.ChallengeProjects, c => c.ChallengeProjects.Select(cp => cp.Project));
            foreach (var challenge in challenges)
                challenge.ImageSource = challenge.ImageAttachmentId.HasValue ? _attachmentFileService.GetAttachmentSourceValue(challenge.ImageAttachmentId) : "/PhysicalStorage/no_image.png";

            var pagedList = challenges.ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;
            TempData["PageNumber"] = pageNumber;
            TempData.Keep("PageNumber");

            return View(pagedList);
        }

        /// <summary>
        /// صفحه ی ثبت چالش
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ViewBag.Projects = _projectService.GetList(p => p.IsActive && !p.IsDeleted && p.CreatedBy == SessionData.Current.User.Id);
            return View(new ChallengeCreateCommand());
        }

        /// <summary>
        /// ثبت چالش جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="attachmentImage"></param>
        /// <param name="attachmentVideo"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ChallengeCreateCommand command, HttpPostedFileBase attachmentImage, HttpPostedFileBase attachmentVideo)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Projects = _projectService.GetList(p => p.IsActive && !p.IsDeleted && p.CreatedBy == SessionData.Current.User.Id);
                return View(command);
            }

            var existsTitle = _challengeService.Any(c => !c.IsDeleted && c.Title == command.Title);
            if (existsTitle)
            {
                ViewBag.Projects = _projectService.GetList(p => p.IsActive && !p.IsDeleted && p.CreatedBy == SessionData.Current.User.Id);
                ModelState.AddModelError("Title", Strings.ChallengeModel_ExistsChallengeTitle);
                return View(command);
            }

            #region Attachment file

            #region Attachment image

            if (attachmentImage != null && attachmentImage.ContentLength > 0)
            {
                byte[] imageData;
                var fileSize = attachmentImage.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(attachmentImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(attachmentImage.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(attachmentImage.FileName));
                var imageWidth = 0;
                var imageHeight = 0;
                if (mimeType.Contains("image"))
                {
                    var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                    imageWidth = img.Width;
                    imageHeight = img.Height;
                }

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(attachmentImage.FileName),
                    System.IO.Path.GetExtension(attachmentImage.FileName), fileSize, mimeType,
                    imageWidth, imageHeight, "DepartmentImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.ImageAttachmentId = attachmentId;
            }
            #endregion

            #region Attachment video

            if (attachmentVideo != null && attachmentVideo.ContentLength > 0)
            {
                byte[] imageData;
                var fileSize = attachmentVideo.ContentLength;
                using (var binaryReader = new System.IO.BinaryReader(attachmentVideo.InputStream))
                {
                    imageData = binaryReader.ReadBytes(attachmentVideo.ContentLength);
                }

                var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(attachmentVideo.FileName));

                var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(attachmentVideo.FileName),
                    System.IO.Path.GetExtension(attachmentVideo.FileName), fileSize, mimeType,
                    0, 0, "DepartmentImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                command.VideoAttachmentId = attachmentId;
            }
            #endregion

            #endregion

            var newChallengeId = _challengeService.CreateNewChallenge(command, SessionData.Current.User.Id);

            #region Assign projects to challenge
            if(command.ProjectsId != null)
            {
                command.ProjectsId.RemoveAll(p => p == Guid.Empty);
                if (command.ProjectsId.Count > 0)
                    _challengeProjectService.AssignProjectsToChallenge(newChallengeId, command.ProjectsId, SessionData.Current.User.Id);
            }
            #endregion

            _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "ChallengeController", "Create", "Success Create Challenge", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// صفحه ی ویرایش چالش
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {
            var challenge = _challengeService.GetList(c => c.Id == id, null, c => c.ChallengeProjects).FirstOrDefault();
            if (challenge == null)
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            ViewBag.Projects = _projectService.GetList(p => p.IsActive && !p.IsDeleted && p.CreatedBy == SessionData.Current.User.Id);

            #region Fill projects
            List<string> projectsId = new List<string>();
            foreach (var item in challenge.ChallengeProjects)
            {
                projectsId.Add(item.ProjectId.ToString());
            }
            ViewBag.SelectedTags = string.Join(",", projectsId);
            #endregion

            return View(new ChallengeEditCommand()
            {
                Id = challenge.Id,
                Title = challenge.Title,
                Description = challenge.Description,
                ProblemDescription = challenge.ProblemDescription,
                ProblemSolvingDescription = challenge.ProblemSolvingDescription,
                ImageAttachmentId = challenge.ImageAttachmentId,
                ImageSource = _attachmentFileService.GetAttachmentSourceValue(challenge.ImageAttachmentId),
                VideoAttachmentId = challenge.VideoAttachmentId,
                VideoSource = _attachmentFileService.GetAttachmentSourceValue(challenge.VideoAttachmentId),
                IsActive = challenge.IsActive
            });
        }

        /// <summary>
        /// ویرایش یک چالش
        /// </summary>
        /// <param name="command"></param>
        /// <param name="attachmentImage"></param>
        /// <param name="attachmentVideo"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ChallengeEditCommand command, HttpPostedFileBase attachmentImage, HttpPostedFileBase attachmentVideo)
        {
            var challenge = _challengeService.GetList(c => c.Id == command.Id, null, c => c.ChallengeProjects).FirstOrDefault().MapToEntity();
            if (challenge == null)
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            if (ModelState.IsValid)
            {
                var existsTitle = _challengeService.Any(c => !c.IsDeleted && c.Title == command.Title && c.Id != command.Id);
                if (existsTitle)
                {
                    ViewBag.Projects = _projectService.GetList(p => p.IsActive && !p.IsDeleted && p.CreatedBy == SessionData.Current.User.Id);
                    ModelState.AddModelError("Title", Strings.ChallengeModel_ExistsChallengeTitle);
                    return View(command);
                }

                #region Attachment file

                #region Attachment image

                if (attachmentImage != null && attachmentImage.ContentLength > 0)
                {
                    if (challenge.ImageAttachmentId.HasValue)
                        _attachmentFileService.RemoveAttachment(challenge.ImageAttachmentId.Value);

                    byte[] imageData;
                    var fileSize = attachmentImage.ContentLength;
                    using (var binaryReader = new System.IO.BinaryReader(attachmentImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(attachmentImage.ContentLength);
                    }

                    var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(attachmentImage.FileName));
                    var imageWidth = 0;
                    var imageHeight = 0;
                    if (mimeType.Contains("image"))
                    {
                        var img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageData));
                        imageWidth = img.Width;
                        imageHeight = img.Height;
                    }

                    var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(attachmentImage.FileName),
                        System.IO.Path.GetExtension(attachmentImage.FileName), fileSize, mimeType,
                        imageWidth, imageHeight, "DepartmentImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                    command.ImageAttachmentId = attachmentId;
                }
                #endregion

                #region Attachment video

                if (attachmentVideo != null && attachmentVideo.ContentLength > 0)
                {
                    if (challenge.VideoAttachmentId.HasValue)
                        _attachmentFileService.GetAttachmentSourceValue(challenge.VideoAttachmentId.Value);

                    byte[] imageData;
                    var fileSize = attachmentVideo.ContentLength;
                    using (var binaryReader = new System.IO.BinaryReader(attachmentVideo.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(attachmentVideo.ContentLength);
                    }

                    var mimeType = MimeTypeHelper.GetMimeType(System.IO.Path.GetExtension(attachmentVideo.FileName));

                    var attachmentId = _attachmentFileService.AddAttachment(SessionData.Current.User.Id, Guid.NewGuid(), System.IO.Path.GetFileName(attachmentVideo.FileName),
                        System.IO.Path.GetExtension(attachmentVideo.FileName), fileSize, mimeType,
                        0, 0, "DepartmentImage - " + Guid.NewGuid(), "", imageData, DateTime.Now);

                    command.VideoAttachmentId = attachmentId;
                }
                #endregion

                #endregion

                _challengeService.UpdateChallenge(challenge, command, SessionData.Current.User.Id);

                #region Assign projects to challenge
                if(command.ProjectsId != null)
                {
                    command.ProjectsId.RemoveAll(p => p == Guid.Empty);
                    if (command.ProjectsId.Count > 0)
                        _challengeProjectService.AssignProjectsToChallenge(challenge.Id, command.ProjectsId, SessionData.Current.User.Id);
                }
                #endregion

                _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "ChallengeController", "Edit", "Success Edit Challenge", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
            }

            #region Fill projects
            List<string> projectsId = new List<string>();
            foreach (var item in challenge.ChallengeProjects)
            {
                projectsId.Add(item.ProjectId.ToString());
            }
            ViewBag.SelectedTags = string.Join(",", projectsId);
            #endregion

            return View(command);
        }

        /// <summary>
        /// تغییر وضعیت فعال بودن
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ChangeStatus(Guid id)
        {
            var challenge = _challengeService.Get(c => c.Id == id).MapToEntity();
            if (challenge == null)
                return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });

            challenge.IsActive = !challenge.IsActive;
            _challengeService.Update(challenge);
            _challengeService.Save();
            return RedirectToAction("Index", new { pageNumber = TempData.ContainsKey("PageNumber") ? int.Parse(TempData["PageNumber"].ToString()) : 1 });
        }

        /// <summary>
        /// حذف چالش
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var challenge = _challengeService.Get(id).MapToEntity();

                #region Remove dependecies

                var challengesProject = _challengeProjectService.GetList(cp => cp.ChallengeId == challenge.Id);
                foreach (var challengeProject in challengesProject)
                {
                    _challengeProjectService.Delete(challengeProject.Id);
                }
                _challengeProjectService.Save();
                #endregion

                _challengeService.Delete(challenge.Id);
                _challengeService.Save();
                return Json(new
                {
                    Message = Strings.Delete_Challenge_Successfull,
                    Success = Strings.Success,
                    Type = "success"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Message = Strings.Global_SystemError,
                    Success = Strings.Global_Error,
                    Type = "error"
                });
            }
        }
    }
}