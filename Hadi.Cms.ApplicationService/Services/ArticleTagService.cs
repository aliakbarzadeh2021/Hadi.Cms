using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Hadi.Cms.ApplicationService.Services
{
    public class ArticleTagService
    {
        private DataContext _dataContext;

        public ArticleTagService()
        {
            _dataContext = new DataContext();
        }

        /// <summary>
        /// نسبت دادن برچسب به مقاله
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="tagIds"></param>
        /// <param name="userId"></param>
        public void AssignTagsToArticle(Guid articleId, List<Guid> tagIds, Guid userId)
        {
            // Delete old tag
            var oldTags = _dataContext.ArticleTagRepository.GetList(at => at.ArticleId == articleId);
            _dataContext.ArticleTagRepository.DeleteRange(oldTags);

            if (tagIds != null)
            {
                foreach (var tag in tagIds)
                {
                    // Add new ddd tag to article
                    var newArticleTag = new ArticleTag()
                    {
                        ArticleId = articleId,
                        TagId = tag,
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                        ModifiedBy = userId,
                        ModifiedDate = DateTime.Now,

                    };
                    _dataContext.ArticleTagRepository.Insert(newArticleTag);
                }
            }
            _dataContext.Save();
        }

        public IArticleTagDto Get(Expression<Func<ArticleTag, bool>> filter)
        {
            var articleTag = _dataContext.ArticleTagRepository.Get(filter);
            return articleTag.MapToDto();
        }

        public IArticleTagDto GetById(object Id)
        {
            var articleTag = _dataContext.ArticleTagRepository.GetByID(Id);
            return articleTag.MapToDto();
        }

        public List<IArticleTagDto> GetList(Expression<Func<ArticleTag, bool>> filter = null, Func<IQueryable<ArticleTag>, IOrderedQueryable<ArticleTag>> orderBy = null, params Expression<Func<ArticleTag, object>>[] includes)
        {
            var articleTags = _dataContext.ArticleTagRepository.GetList(filter, orderBy, includes);
            return articleTags.MapToListDto();
        }

        public void Insert(ArticleTag model)
        {
            _dataContext.ArticleTagRepository.Insert(model);
        }

        public void Update(ArticleTag model)
        {
            _dataContext.ArticleTagRepository.Update(model);
        }

        public void Delete(ArticleTag model)
        {
            _dataContext.ArticleTagRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.ArticleTagRepository.Delete(Id);
        }

        public void DeleteRange(IEnumerable<ArticleTag> entities)
        {
            _dataContext.ArticleTagRepository.DeleteRange(entities);
        }

        public IQueryable<IArticleTagDto> Where(Expression<Func<ArticleTag, bool>> filter)
        {
            var articleTags = _dataContext.ArticleTagRepository.Where(filter);
            var articleTagsDto = articleTags.Select(q => q.MapToDto());
            return articleTagsDto;
        }

        public bool Any(Expression<Func<ArticleTag, bool>> filter)
        {
            return _dataContext.ArticleTagRepository.Any(filter);
        }

        public int Count(Expression<Func<ArticleTag, bool>> where = null)
        {
            return _dataContext.ArticleTagRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }

    }
}
