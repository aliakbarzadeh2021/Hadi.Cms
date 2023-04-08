using Hadi.Cms.ApplicationService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.ApplicationService.QueryModels;
using Hadi.Cms.Infrastructure.Helpers;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Model.Mappings.Mappers;

namespace Hadi.Cms.Web.Controllers
{
    /// <summary>
    /// article controller
    /// </summary>
    public class ArticlesController : Controller
    {
        private readonly ArticleService _articleService;
        private readonly AttachmentFileService _attachmentFileService;
        private readonly TagService _tagService;
        private readonly AuthorService _authorService;
        public ArticlesController()
        {
            _articleService = new ArticleService();
            _attachmentFileService = new AttachmentFileService();
            _tagService = new TagService();
            _authorService = new AuthorService();
        }

        // <summary>
        // دریافت لیست مقالات
        // </summary>
        // <param name "id"></param>
        // <returns></returns>
        public ActionResult Index()
        {
            var articles = _articleService.GetList(a => a.IsActive && !a.IsDeleted);

            #region Map to dto

            var dto = new ArticleContainerDto
            {
                Articles = articles.Select(a => new AllArticleDto()
                {
                    Id = a.Id,
                    Title = a.Title,
                    ArticleImageSource = _attachmentFileService.GetAttachmentSourceValue(a.AttachmentImageId),
                    AuthorName = a.AuthorId.HasValue ? _authorService.GetById(a.AuthorId.Value)?.FullName : "",
                    AuthorImageSource = a.AuthorId.HasValue ? _attachmentFileService.GetAttachmentSourceValue(_authorService.GetById(a.AuthorId.Value)?.AuthorImageGuid) : "",
                    ShamsiCreatedDate = a.CreatedDate.ToPersianDate(),
                    CreatedDate = a.CreatedDate,
                    IsSpecial = a.IsSpecial,
                    IsSpecialCreatedDate = a.IsSpecialCreatedDate,
                    Tags = a.ArticleTagsDto.Select(at => at.TagDto).Where(t => t.IsActive).ToList()
                }).OrderByDescending(a => a.CreatedDate).ToList()
            };
            dto.SpecialArticle = dto.Articles.Where(a => a.IsSpecial).OrderByDescending(a => a.IsSpecialCreatedDate.Value).FirstOrDefault();
            dto.HaveSpecialArticle = dto.SpecialArticle != null;

            if (dto.SpecialArticle != null)
                dto.Articles.Remove(dto.SpecialArticle);

            #endregion

            return View(dto);
        }

        /// <summary>
        /// جستجوی مقالات
        /// </summary>
        /// <param name="id"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public ActionResult FilterArticles(Guid? id = null, string search = "")
        {
            var dto = _articleService.Search(id, search);

            var specialArticle = dto.Where(a => a.IsSpecial).Select(a => new AllArticleDto()
            {
                Id = a.Id,
                Title = a.Title,
                ArticleImageSource = _attachmentFileService.GetAttachmentSourceValue(a.ArticleImageGuid),
                AuthorName = a.AuthorId.HasValue ? _authorService.GetById(a.AuthorId.Value)?.FullName : "",
                AuthorImageSource = a.AuthorId.HasValue ? _attachmentFileService.GetAttachmentSourceValue(_authorService.GetById(a.AuthorId.Value)?.AuthorImageGuid) : "",
                ShamsiCreatedDate = a.CreatedDate.ToPersianDate(),
                CreatedDate = a.CreatedDate,
                IsSpecial = a.IsSpecial,
                IsSpecialCreatedDate = a.IsSpecialCreatedDate,
                Tags = a.Tags.ToList(),
                AuthorImageGuid = a.AuthorImageGuid,
                ArticleImageGuid = a.ArticleImageGuid
            }).FirstOrDefault();

            if (specialArticle != null)
                dto = dto.Where(d => d.Id != specialArticle.Id).ToList();

            return Json(new
            {
                #region Top articles

                TopArticles = dto.Take(specialArticle != null ? 5 : 8).Select(u => new
                {
                    Id = u.Id,
                    Title = u.Title,
                    ArticleImageSource = _attachmentFileService.GetAttachmentSourceValue(u.ArticleImageGuid),
                    ShamsiCreatedDate = u.ShamsiCreatedDate,
                    Tags = u.Tags.Select(t => new
                    {
                        Title = t.Title,
                        Color = t.Color
                    }).ToList(),
                    IsSpecial = u.IsSpecial,
                    IsSpecialCreatedDate = u.IsSpecialCreatedDate,
                    AuthorName = u.AuthorId.HasValue ? _authorService.GetById(u.AuthorId.Value)?.FullName : "",
                    AuthorImageSource = u.AuthorId.HasValue ? _attachmentFileService.GetAttachmentSourceValue(_authorService.GetById(u.AuthorId.Value)?.AuthorImageGuid) : "",
                }).OrderByDescending(u => u.IsSpecial).ToList(),

                #endregion

                #region Below articles

                BelowArticles = dto.Skip(specialArticle != null ? 5 : 8).Take(8).Select(b => new
                {
                    Id = b.Id,
                    Title = b.Title,
                    ArticleImageSource = _attachmentFileService.GetAttachmentSourceValue(b.ArticleImageGuid),
                    ShamsiCreatedDate = b.ShamsiCreatedDate,
                    Tags = b.Tags.Select(t => new
                    {
                        Title = t.Title,
                        Color = t.Color
                    }).ToList(),
                    IsSpecial = b.IsSpecial,
                    IsSpecialCreatedDate = b.IsSpecialCreatedDate,
                    AuthorName = b.AuthorId.HasValue ? _authorService.GetById(b.AuthorId.Value)?.FullName : "",
                    AuthorImageSource = b.AuthorId.HasValue ? _attachmentFileService.GetAttachmentSourceValue(_authorService.GetById(b.AuthorId.Value)?.AuthorImageGuid) : "",
                }).OrderByDescending(b => b.IsSpecial).ToList(),

                #endregion

                #region Special article
                SpecialArticle = specialArticle != null ? new
                {
                    Id = specialArticle.Id,
                    Title = specialArticle.Title,
                    ArticleImageSource = _attachmentFileService.GetAttachmentSourceValue(specialArticle.ArticleImageGuid),
                    ShamsiCreatedDate = specialArticle.ShamsiCreatedDate,
                    Tags = specialArticle.Tags.Select(t => new
                    {
                        Title = t.Title,
                        Color = t.Color
                    }).ToList(),
                    IsSpecial = specialArticle.IsSpecial,
                    IsSpecialCreatedDate = specialArticle.IsSpecialCreatedDate,
                    AuthorName = specialArticle.AuthorName,
                    AuthorImageSource = specialArticle.AuthorImageSource
                } : null,
                #endregion

                HaveSpecialArticle = dto.Any(d => d.IsSpecial),
                Count = _articleService.Count(a => a.IsActive && !a.IsDeleted)
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// دریافت لیست مقالات - دکمه مقالات بیشتر
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tagId"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public ActionResult GetArticles(int id = 1, Guid? tagId = null, string search = "")
        {
            var articles = _articleService.Search(tagId, search);

            articles = articles.Take(16 * id).ToList();

            var specialArticle = articles.Where(a => a.IsSpecial).Select(a => new AllArticleDto()
            {
                Id = a.Id,
                Title = a.Title,
                ArticleImageSource = _attachmentFileService.GetAttachmentSourceValue(a.ArticleImageGuid),
                AuthorName = a.AuthorId.HasValue ? _authorService.GetById(a.AuthorId.Value)?.FullName : "",
                AuthorImageSource = a.AuthorId.HasValue ? _attachmentFileService.GetAttachmentSourceValue(_authorService.GetById(a.AuthorId.Value)?.AuthorImageGuid) : "",
                ShamsiCreatedDate = a.CreatedDate.ToPersianDate(),
                CreatedDate = a.CreatedDate,
                IsSpecial = a.IsSpecial,
                IsSpecialCreatedDate = a.IsSpecialCreatedDate,
                Tags = a.Tags.ToList()
            }).FirstOrDefault();

            if (specialArticle != null)
                articles = articles.Where(d => d.Id != specialArticle.Id).ToList();

            return Json(new
            {
                #region Top articles
                TopArticles = articles.Take(specialArticle != null ? 5 : 8).Select(a => new
                {
                    Id = a.Id,
                    Title = a.Title,
                    ArticleImageSource = _attachmentFileService.GetAttachmentSourceValue(a.ArticleImageGuid),
                    AuthorName = a.AuthorId.HasValue ? _authorService.GetById(a.AuthorId.Value)?.FullName : "",
                    AuthorImageSource = a.AuthorId.HasValue ? _attachmentFileService.GetAttachmentSourceValue(_authorService.GetById(a.AuthorId.Value)?.AuthorImageGuid) : "",
                    ShamsiCreatedDate = a.CreatedDate.ToPersianDate(),
                    CreatedDate = a.CreatedDate,
                    IsSpecial = a.IsSpecial,
                    IsSpecialCreatedDate = a.IsSpecialCreatedDate,
                    Tags = a.Tags.Select(t => new
                    {
                        Title = t.Title,
                        Color = t.Color
                    }).ToList()
                }).ToList(),
                #endregion
                #region Below articles
                BelowArticles = articles.Skip(specialArticle != null ? 5 : 8).Select(b => new
                {
                    Id = b.Id,
                    Title = b.Title,
                    ArticleImageSource = _attachmentFileService.GetAttachmentSourceValue(b.ArticleImageGuid),
                    AuthorName = b.AuthorId.HasValue ? _authorService.GetById(b.AuthorId.Value)?.FullName : "",
                    AuthorImageSource = b.AuthorId.HasValue ? _attachmentFileService.GetAttachmentSourceValue(_authorService.GetById(b.AuthorId.Value)?.AuthorImageGuid) : "",
                    ShamsiCreatedDate = b.CreatedDate.ToPersianDate(),
                    CreatedDate = b.CreatedDate,
                    IsSpecial = b.IsSpecial,
                    IsSpecialCreatedDate = b.IsSpecialCreatedDate,
                    Tags = b.Tags.Select(t => new
                    {
                        Title = t.Title,
                        Color = t.Color
                    }).ToList(),
                }).OrderByDescending(b => b.CreatedDate).ToList(),
                #endregion
                HaveSpecialArticle = articles.Any(ar => ar.IsSpecial),

            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(Guid Id)
        {
            var article = _articleService.GetList(q => !q.IsDeleted && q.IsActive && q.Id == Id, null, q => q.ArticleTags, q => q.ArticleTags.Select(at => at.Tag)).FirstOrDefault();
            if (article == null)
                return RedirectToAction("Index");

            article.ReviewCount += 1;
            _articleService.Update(article.MaptoEntity());
            _articleService.Save();

            IAuthorDto author = null;
            if (article.AuthorId.HasValue)
                author = _authorService.GetById(article.AuthorId.Value);

            var dto = new AnArticleDto()
            {
                Id = article.Id,
                Title = article.Title,
                AuthorName = author?.FullName,
                AuthorImageSource = _attachmentFileService.GetAttachmentSourceValue(author?.AuthorImageGuid),
                ArticleImageSource = _attachmentFileService.GetAttachmentSourceValue(article.AttachmentImageId),
                Source = article.Source,
                ShamsiStringDate = article.CreatedDate.ToPersianDate(),
                Summary = article.Summary,
                Tags = article.ArticleTagsDto.Select(at => at.TagDto).Where(t => t.IsActive).ToList(),
                ImageCaption = article.ImageCaption,
                InstagramAddress = author?.InstagramAddress,
                TelegramAddress = author?.TelegramAddress,
                LinkedInAddress = author?.LinkedInAddress
            };

            return View(dto);
        }

        [ChildActionOnly]
        public ActionResult RelatedArticlesPartial(Guid Id)
        {
            var relatedArticles = _articleService.GetRelatedArticles(Id);

            var dto = relatedArticles.Select(a => new AllArticleDto()
            {
                Id = a.Id,
                Title = a.Title,
                ArticleImageSource = _attachmentFileService.GetAttachmentSourceValue(a.AttachmentImageId),
                AuthorName = a.AuthorId.HasValue ? _authorService.GetById(a.AuthorId.Value)?.FullName : "",
                AuthorImageSource = a.AuthorId.HasValue ? _attachmentFileService.GetAttachmentSourceValue(_authorService.GetById(a.AuthorId.Value)?.AuthorImageGuid) : "",
                ShamsiCreatedDate = a.CreatedDate.ToPersianDate(),
                CreatedDate = a.CreatedDate,
                IsSpecial = a.IsSpecial,
                Tags = a.ArticleTagsDto.Select(at => at.TagDto).Where(t => t.IsActive).ToList()
            }).ToList();
            return PartialView(dto);
        }

        [ChildActionOnly]
        public ActionResult PopularArticlePartial(Guid currentArticleId)
        {
            var popularArticles = _articleService.GetPopularArticles(currentArticleId);

            var dto = popularArticles.Select(a => new AllArticleDto()
            {
                Id = a.Id,
                Title = a.Title,
                ArticleImageSource = _attachmentFileService.GetAttachmentSourceValue(a.AttachmentImageId),
                AuthorName = a.AuthorId.HasValue ? _authorService.GetById(a.AuthorId.Value)?.FullName : "",
                AuthorImageSource = a.AuthorId.HasValue ? _attachmentFileService.GetAttachmentSourceValue(_authorService.GetById(a.AuthorId.Value)?.AuthorImageGuid) : "",
                ShamsiCreatedDate = a.CreatedDate.ToPersianDate(),
                CreatedDate = a.CreatedDate,
                IsSpecial = a.IsSpecial,
                Tags = a.ArticleTagsDto.Select(at => at.TagDto).ToList()
            }).ToList();

            return PartialView("_PapularArticle", dto);
        }

        [ChildActionOnly]
        public ActionResult LatestArticlePartial()
        {
            var article = _articleService.GetList(q => !q.IsDeleted && q.IsActive).OrderByDescending(q => q.CreatedDate).FirstOrDefault();
            return PartialView(article);
        }
        [ChildActionOnly]
        public ActionResult SpecialArticlePartial(int id)
        {
            var articles = _articleService.GetList(a => a.IsSpecial && a.IsActive && !a.IsDeleted, null,
                a => a.ArticleTags, a => a.ArticleTags.Select(at => at.Tag));

            var dto = articles.Select(a => new AllArticleDto()
            {
                Id = a.Id,
                Title = a.Title,
                ArticleImageSource = _attachmentFileService.GetAttachmentSourceValue(a.AttachmentImageId),
                AuthorName = a.AuthorId.HasValue ? _authorService.GetById(a.AuthorId.Value)?.FullName : "",
                AuthorImageSource = a.AuthorId.HasValue ? _attachmentFileService.GetAttachmentSourceValue(_authorService.GetById(a.AuthorId.Value)?.AuthorImageGuid) : "",
                ShamsiCreatedDate = a.CreatedDate.ToPersianDate(),
                CreatedDate = a.CreatedDate,
                IsSpecial = a.IsSpecial,
                IsSpecialCreatedDate = a.CreatedDate,
                Tags = a.ArticleTagsDto.Select(at => at.TagDto).ToList()
            }).OrderByDescending(o => o.IsSpecialCreatedDate).Take(id).ToList();

            return PartialView("_SpecialArticlePartial", dto);
        }
        [ChildActionOnly]
        public ActionResult GridEducationPartial(Guid? id)
        {
            List<IAttachmentFileDto> filesList = new List<IAttachmentFileDto>();

            var extensions = new[] { ".mkv", ".3gp", ".mp4" };

            if (id == null)
                filesList = _attachmentFileService.GetList(a => extensions.Any(m => m == a.Extension), null, a => a.AttachmentFileTags, a => a.AttachmentFileTags.Select(at => at.Tag));
            else
                filesList.AddRange(_attachmentFileService
                       .GetList(a => a.AttachmentFileTags.Any(at => at.TagId == id) && extensions.Any(m => m == a.MimeType), null,
                           a => a.AttachmentFileTags, a => a.AttachmentFileTags.Select(at => at.Tag)));

            var dto = filesList.OrderByDescending(a => a.LastModified).DistinctBy(a => a.Id).Take(3).Select(a => new AttachmentFileTagDto()
            {
                Source = _attachmentFileService.GetAttachmentSourceValue(a.Id),
                Poster = _attachmentFileService.GetAttachmentSourceValue(_attachmentFileService.Get(f => f.Id == a.PosterImageGuid)?.Id),
                MimeType = a.MimeType
            }).ToList();
            return PartialView("_GridEducationPartial", dto);
        }

        public ActionResult NlEmailPartial()
        {
            return PartialView("_NlEmailPartial", new NlEmailCreateCommand());
        }

    }
}