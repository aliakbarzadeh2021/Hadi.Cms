using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Ajax.Utilities;
using Hadi.Cms.ApplicationService.QueryModels;
using Hadi.Cms.Infrastructure.Helpers;

namespace Hadi.Cms.ApplicationService.Services
{
    public class ArticleService
    {
        private DataContext _dataContext;

        public ArticleService()
        {
            _dataContext = new DataContext();
        }

        public IArticleDto Get(Expression<Func<Article, bool>> filter, params Expression<Func<Article, object>>[] includes)
        {
            var article = _dataContext.ArticleRepository.Get(filter, includes);
            return article.MapToDto();
        }

        public IArticleDto GetById(object Id)
        {
            var article = _dataContext.ArticleRepository.GetByID(Id);
            return article.MapToDto();
        }

        public List<IArticleDto> GetList(Expression<Func<Article, bool>> filter = null, Func<IQueryable<Article>, IOrderedQueryable<Article>> orderBy = null, params Expression<Func<Article, object>>[] includes)
        {
            var articles = _dataContext.ArticleRepository.GetList(filter, orderBy, includes);
            return articles.MapToListDto();
        }

        /// <summary>
        /// جسنجوی مفالات
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="strSearch"></param>
        /// <returns></returns>
        public List<AllArticleDto> Search(Guid? tagId, string strSearch)
        {
            var articles = new List<IArticleDto>();
            if (tagId != null)
            {
                var tag = _dataContext.TagRepository.Get(t => t.Id == tagId);
                if (tag != null)
                {
                    articles.AddRange(GetList(a => a.IsActive && !a.IsDeleted &&
                            a.ArticleTags.Any(at => at.TagId == tag.Id) && a.Title.Contains(strSearch), null,
                        a => a.ArticleTags, a => a.ArticleTags.Select(at => at.Tag)));

                    articles = articles.DistinctBy(d => d.Id).ToList();
                }
            }

            else
                articles.AddRange(GetList(a => a.IsActive && !a.IsDeleted && a.Title.Contains(strSearch), null, a => a.ArticleTags, a => a.ArticleTags.Select(at => at.Tag)));

            #region Map to dto
            var dto = articles.Select(a => new AllArticleDto()
            {
                Id = a.Id,
                AuthorId = a.AuthorId,
                Title = a.Title,
                ArticleImageGuid = a.AttachmentImageId,
                AuthorName = _dataContext.UserRepository.Get(u => u.Id == a.CreatedBy)?.FullName,
                AuthorImageGuid = a.CreatedBy,
                Tags = a.ArticleTagsDto.Select(at => at.TagDto).ToList(),
                CreatedDate = a.CreatedDate,
                ShamsiCreatedDate = a.CreatedDate.ToPersianDate(),
                IsSpecialCreatedDate = a.IsSpecialCreatedDate,
                IsSpecial = a.IsSpecial
            }).OrderByDescending(a => a.CreatedDate).ToList();
            #endregion

            return dto;
        }

        public void Insert(Article model)
        {
            _dataContext.ArticleRepository.Insert(model);
        }

        public void Update(Article model)
        {
            _dataContext.ArticleRepository.Update(model);
        }

        public void Delete(Article model)
        {
            _dataContext.ArticleRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.ArticleRepository.Delete(Id);
        }

        public void DeleteRange(IEnumerable<Article> entities)
        {
            _dataContext.ArticleRepository.DeleteRange(entities);
        }

        public IQueryable<IArticleDto> Where(Expression<Func<Article, bool>> filter)
        {
            var articles = _dataContext.ArticleRepository.Where(filter);
            var articlesDto = articles.Select(q => q.MapToDto());
            return articlesDto;
        }

        public bool Any(Expression<Func<Article, bool>> filter)
        {
            return _dataContext.ArticleRepository.Any(filter);
        }

        public int Count(Expression<Func<Article, bool>> where = null)
        {
            return _dataContext.ArticleRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }


        public List<IArticleDto> GetRelatedArticles(Guid articleId, int take = 3)
        {
            var article = _dataContext.ArticleRepository.GetList(q => !q.IsDeleted && q.IsActive && q.Id == articleId, null, q => q.ArticleTags, q => q.ArticleTags.Select(at => at.Tag)).FirstOrDefault().MapToDto();
            var tags = article.ArticleTagsDto.Select(at => at.TagDto).Where(t => t.IsActive).ToList();
            var relatedArticles = new List<IArticleDto>();
            foreach (var tag in tags)
            {
                var parentTag =
                        _dataContext.TagRepository.Get(t => !t.IsDeleted && t.Id == tag.ParentId);
                var childrenTag =
                    _dataContext.TagRepository.GetList(
                        t => t.ParentId == parentTag.Id && t.IsActive && !t.IsDeleted);
                foreach (var child in childrenTag)
                {
                    var articles = _dataContext.ArticleRepository.GetList(a => a.ArticleTags.Any(at => at.TagId == child.Id) && a.Id != articleId && a.IsActive && !a.IsDeleted, null, at => at.ArticleTags, at => at.ArticleTags.Select(a => a.Tag));
                    relatedArticles.AddRange(articles.MapToListDto());
                }
            }

            //relatedArticles = relatedArticles.OrderByDescending(r => r.ArticleTagsDto.Any(at => at.))

            return relatedArticles;
        }

        /// <summary>
        /// دریافت مقالات پر بازدید
        /// </summary>
        /// <param name="take"></param>
        /// <param name="currentArticleId"></param>
        /// <returns></returns>
        public List<IArticleDto> GetPopularArticles(Guid currentArticleId, int take = 3)
        {
            var popularArticles = _dataContext.ArticleRepository.GetList(a => a.IsActive && !a.IsDeleted && a.ReviewCount > 0 && a.Id != currentArticleId, null, a => a.ArticleTags, a => a.ArticleTags.Select(at => at.Tag))
                .OrderByDescending(o => o.ReviewCount).Take(take).ToList().MapToListDto();

            foreach (var popularArticle in popularArticles.ToList())
            {
                popularArticle.ArticleTagsDto = popularArticle.ArticleTagsDto.Where(at => at.TagDto.IsActive).ToList();
            }

            return popularArticles;
        }

    }
}
