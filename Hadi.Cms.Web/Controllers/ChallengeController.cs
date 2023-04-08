using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Hadi.Cms.ApplicationService.QueryModels;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Web.Controllers
{
    public class ChallengeController : Controller
    {
        private readonly ChallengeService _challengeService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly ProjectService _projectService;

        public ChallengeController()
        {
            _challengeService = new ChallengeService();
            _attachmentFileService = new AttachmentFileService();
            _projectService = new ProjectService();
        }

        public ActionResult Details(Guid id)
        {
            var challenge = _challengeService.GetList(c => c.Id == id && c.IsActive && !c.IsDeleted, null, c => c.ChallengeProjects, c => c.ChallengeProjects.Select(cp => cp.Challenge), c => c.ChallengeProjects.Select(cp => cp.Project), c => c.ChallengeProjects.Select(cp => cp.Project.ProjectTags), c => c.ChallengeProjects.Select(cp => cp.Project.ProjectTags.Select(pt => pt.Tag))).FirstOrDefault();
            if (challenge == null)
                return HttpNotFound("Challenge not found .");

            #region Another challenges

            var anotherChallenges = new List<IChallengeDto>();
            var project = challenge.ChallengeProjects.Select(cp => cp.Project).FirstOrDefault();
            if (project != null)
            {
                foreach (var projectTag in project.ProjectTags)
                {
                    var projects = _projectService.GetList(p => p.ProjectTags.Any(pt => pt.TagId == projectTag.TagId), null, p => p.ChallengeProjects, p => p.ChallengeProjects.Select(cp => cp.Challenge));

                    var challenges = projects.SelectMany(p => p.ChallengeProject, (p, cp) => cp.Challenge).ToList();

                    anotherChallenges.AddRange(challenges);
                }

                anotherChallenges = anotherChallenges.Where(a => a.Id != challenge.Id).DistinctBy(a => a.Id).OrderByDescending(a => a.CreatedDate).Take(4).ToList();
            }

            #endregion

            challenge.VideoSource = _attachmentFileService.GetAttachmentSourceValue(challenge.VideoAttachmentId);
            challenge.ImageSource = _attachmentFileService.GetAttachmentSourceValue(challenge.ImageAttachmentId);

            foreach (var item in anotherChallenges)
            {
                item.ProjectName = item.ChallengeProjects.FirstOrDefault()?.Project.Title;
                item.VideoSource = _attachmentFileService.GetAttachmentSourceValue(item.VideoAttachmentId);
                item.ImageSource = _attachmentFileService.GetAttachmentSourceValue(item.ImageAttachmentId);
            }

            var dto = new ChallengeDetailWithProjectDto
            {
                ProjectDto = challenge.ChallengeProjects.Select(c => c.Project).FirstOrDefault(),
                ChallengeDto = challenge,
                AnotherChallenges = anotherChallenges
            };

            dto.ProjectDto.Source = _attachmentFileService.GetAttachmentSourceValue(dto.ProjectDto.ImageGuid);

            return View(dto);
        }
    }
}