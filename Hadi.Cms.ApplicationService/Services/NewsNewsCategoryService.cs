using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Repository.UnitOfWork;

namespace Hadi.Cms.ApplicationService.Services
{
    /// <summary>
    /// سرویس دسته بندی خبر
    /// </summary>
    public class NewsNewsCategoryService
    {
        private readonly DataContext _dataContext;

        public NewsNewsCategoryService()
        {
            _dataContext = new DataContext();
        }

        ~NewsNewsCategoryService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست دسته بندی های خبر
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<INewsNewsCategory> GetList(Expression<Func<NewsNewsCategory, bool>> filter = null,
            Func<IQueryable<NewsNewsCategory>, IOrderedQueryable<NewsNewsCategory>> orderBy = null,
            params Expression<Func<NewsNewsCategory, object>>[] includes)
        {
            return _dataContext.NewsNewsCategoryRepository.GetList(filter, orderBy, includes).MapToListDto();
        }


        /// <summary>
        /// حذف دسته بندی از خبر
        /// </summary>
        /// <param name="newsNewsCategoryId"></param>
        public void Delete(Guid newsNewsCategoryId)
        {
            _dataContext.NewsNewsCategoryRepository.Delete(newsNewsCategoryId);
        }

        /// <summary>
        /// نسبت دادن دسته بندی به اخبار
        /// </summary>
        /// <param name="newsId"></param>
        /// <param name="newsCategoriesId"></param>
        /// <param name="userId"></param>
        public void AssignCategoriesToNews(Guid newsId, List<Guid> newsCategoriesId, Guid userId)
        {
            var oldCategories = GetList(n => n.NewsId == newsId);
            foreach (var newsNewsCategory in oldCategories)
            {
                Delete(newsNewsCategory.Id);
            }

            if (newsCategoriesId != null && newsCategoriesId.Count > 0)
            {
                foreach (var newCategoryId in newsCategoriesId)
                {
                    var newNewsCategory = new NewsNewsCategory
                    {
                        NewsId = newsId,
                        NewsCategoryId = newCategoryId,
                        CreatedBy = userId,
                        IsActive = true,
                        IsDeleted = false
                    };
                    Insert(newNewsCategory);
                }
                Save();
            }
        }

        public void Insert(NewsNewsCategory entity)
        {
            _dataContext.NewsNewsCategoryRepository.Insert(entity);
        }

        /// <summary>
        /// حذف محدوده ای از دسته بندی ها
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRange(IEnumerable<NewsNewsCategory> entities)
        {
            _dataContext.NewsNewsCategoryRepository.DeleteRange(entities);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
