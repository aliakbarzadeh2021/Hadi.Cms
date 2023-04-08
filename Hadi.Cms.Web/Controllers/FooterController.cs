using System.Linq;
using System.Web.Mvc;
using Hadi.Cms.ApplicationService.QueryModels;
using Hadi.Cms.ApplicationService.Services;

namespace Hadi.Cms.Web.Controllers
{
    /// <summary>
    /// فوتر
    /// </summary>
    public class FooterController : Controller
    {
        private readonly FooterCategoryService _footerCategoryService;
        private readonly FooterCategoryLinkService _footerCategoryLinkService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly ArticleService _articleService;
        private readonly ServiceService _serviceService;

        public FooterController()
        {
            _footerCategoryService = new FooterCategoryService();
            _footerCategoryLinkService = new FooterCategoryLinkService();
            _attachmentFileService = new AttachmentFileService();
            _articleService = new ArticleService();
            _serviceService = new ServiceService();
        }

        public ActionResult FooterCategoryPartial(bool isFooterMobile = false)
        {
            var footerCategories = _footerCategoryService.GetList(f => f.IsActive && !f.IsDeleted);

            #region Map to dto

            var isNotStatisticalRepresentationData = footerCategories.Where(f => !f.StatisticalRepresentation).Select(f =>
                new FooterCategoryWithWithLinkDto()
                {
                    Title = f.Title,
                    Link = f.Link,
                    OrderId = f.OrderId,
                    CreatedDate = f.CreatedDate,
                    FooterLinkDto = _footerCategoryLinkService.GetList(fc => fc.FooterCategoryId == f.Id).Select(fl =>
                        new FooterLinkDto()
                        {
                            Link = fl.Link,
                            LinkText = fl.LinkText,
                            ImageAttachmentGuid = fl.ImageAttachmentGuid,
                            ImageAttachmentSource = _attachmentFileService.GetAttachmentSourceValue(fl.ImageAttachmentGuid),
                            HasImage = fl.ImageAttachmentGuid.HasValue,
                            CreatedDate = fl.CreatedDate
                        }).OrderBy(o => o.CreatedDate).ToList()
                }).ToList();

            var isStatisticalRepresentationData = footerCategories.Where(f => f.StatisticalRepresentation).Select(f =>
                new FooterCategoryWithWithLinkDto()
                {
                    Title = f.Title,
                    Link = f.Link,
                    OrderId = f.OrderId,
                    CreatedDate = f.CreatedDate,
                    FooterLinkDto = f.EntityHaveReviewCount == 1
                        ? _articleService
                            .GetList(a => a.IsActive && !a.IsDeleted && a.ReviewCount > 0, o => o.OrderByDescending(a => a.ReviewCount))
                            .Take(f.NumberOfShows).Select(a => new FooterLinkDto()
                            {
                                Link = "/Articles/Details/" + a.Id,
                                LinkText = a.Title,
                                HasImage = a.AttachmentImageId.HasValue,
                                CreatedDate = a.CreatedDate
                            }).OrderBy(c => c.CreatedDate).ToList()
                        : _serviceService
                            .GetList(s => s.IsActive && !s.IsDeleted && s.ReviewCount > 0, o => o.OrderByDescending(s => s.ReviewCount))
                            .Take(f.NumberOfShows).Select(s => new FooterLinkDto()
                            {
                                Link = "/Services/Details/" + s.Id,
                                LinkText = s.Title?.Replace("<br/>", " "),
                                CreatedDate = s.CreatedDate
                            }).OrderBy(c => c.CreatedDate).ToList()
                }).ToList();

            isNotStatisticalRepresentationData.AddRange(isStatisticalRepresentationData);

            isNotStatisticalRepresentationData = isNotStatisticalRepresentationData.OrderBy(d => d.OrderId).ToList();

            #endregion

            ViewBag.FooterMobile = isFooterMobile;

            return PartialView("_FooterCategoryPartial", isNotStatisticalRepresentationData);

        }
    }
}