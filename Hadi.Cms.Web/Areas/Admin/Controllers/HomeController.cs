using System.Linq;
using Hadi.Cms.Web.Controllers;
using Hadi.Cms.Web.Utilities;
using System.Web.Mvc;
using Hadi.Cms.ApplicationService.QueryModels;
using Hadi.Cms.ApplicationService.Services;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ServiceService _serviceService;
        private readonly ArticleService _articleService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly TagService _tagService;

        public HomeController()
        {
            _articleService = new ArticleService();
            _serviceService = new ServiceService();
            _attachmentFileService = new AttachmentFileService();
            _tagService = new TagService();
        }

        // Admin/Home
        public ActionResult Index()
        {
            if (!SessionData.Current.CurrentUserIsAdministrator)
                return RedirectToAction("Index", "Home", new { area = "Panel" });

            var articles =
                _articleService.GetList(a => !a.IsDeleted && a.IsActive && a.ReviewCount > 0 && a.CreatedBy == SessionData.Current.User.Id,
                    o => o.OrderByDescending(a => a.ReviewCount)).Take(5).ToList();

            foreach (var article in articles)
                article.AttachmentImageSource = _attachmentFileService.GetAttachmentSourceValue(article.AttachmentImageId);

            var services = _serviceService
                .GetList(s => s.IsActive && !s.IsDeleted && s.ReviewCount > 0 && s.CreatedBy == SessionData.Current.User.Id,
                    o => o.OrderByDescending(s => s.ReviewCount)).Take(5).ToList();

            foreach (var service in services)
            {
                service.Title = service.Title?.Replace("<br/>", " ");
                service.AttachmentImageSource = _attachmentFileService.GetAttachmentSourceValue(service.AttachmentImageId);
            }

            var tags = _tagService.GetList(
                t => t.IsActive && !t.IsDeleted && t.CreatedBy == SessionData.Current.User.Id && t.ParentId != null,
                o => o.OrderByDescending(t => t.CreatedDate), t => t.ServiceTags, t => t.ArticleTags, t => t.CourseTags,
                t => t.AttachmentFileTags, t => t.ProjectTags);

            var countOfUse = 0;
            foreach (var tag in tags)
            {
                countOfUse += tag.ArticleTagsDto.Count() +
                              tag.ServiceTagDto.Count() +
                              tag.AttachmentFileTagDto.Count() +
                              tag.CourseTagDto.Count();

                tag.CountOfUse = countOfUse;

                countOfUse = 0;
            }

            tags = tags.Where(p => p.CountOfUse > 0).OrderByDescending(p => p.CountOfUse).ToList();

            var dto = new AdminHomePageDto()
            {
                Articles = articles,
                Services = services,
                Tags = tags
            };

            ViewBag.HasRightToLeftUI = SessionData.Current.HasRightToLeftUI;

            return View(dto);

        }
    }
}