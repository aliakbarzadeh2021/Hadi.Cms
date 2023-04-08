using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Repository.UnitOfWork;

namespace Hadi.Cms.ApplicationService.Services
{
    /// <summary>
    /// سرویس دسته بندی
    /// </summary>
    public class NewsCategoryService
    {
        private readonly DataContext _dataContext;

        public NewsCategoryService()
        {
            _dataContext = new DataContext();
        }

        ~NewsCategoryService()
        {
            _dataContext.Dispose();
        }

        /// <summary>
        /// دریافت لیست دسته بندی
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<INewsCategory> GetList(Expression<Func<NewsCategory, bool>> filter = null,
            Func<IQueryable<NewsCategory>, IOrderedQueryable<NewsCategory>> orderBy = null,
            params Expression<Func<NewsCategory, object>>[] includes)
        {
            var newsCategoryList = _dataContext.NewsCategoryRepository.GetList(filter, orderBy, includes);
            return newsCategoryList.MapToListDto();
        }

        /// <summary>
        /// دریافت یک دسته بندی بر اساس شناسه
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public INewsCategory GetByID(Guid id)
        {
            var newsCategory = _dataContext.NewsCategoryRepository.GetByID(id);
            return newsCategory.MapToDto();
        }

        /// <summary>
        /// دریافت یک دسته بندی بر اساس یک شرط
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public INewsCategory Get(Expression<Func<NewsCategory, bool>> filter = null,
            params Expression<Func<NewsCategory, object>>[] includes)
        {
            var newsCategory = _dataContext.NewsCategoryRepository.Get(filter, includes);
            return newsCategory.MapToDto();
        }

        /// <summary>
        /// ثبت دسته بندی جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        public void CreateNewCategory(NewsCategoryCreateCommand command, Guid userId)
        {
            var newCategory = new NewsCategory
            {
                Title = command.Title,
                EnTitle = command.EnTitle,
                OrderId = command.OrderId,
                IsDeleted = false,
                IsActive = true,
                CreatedBy = userId,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                ModifiedBy = userId
            };
            _dataContext.NewsCategoryRepository.Insert(newCategory);
            _dataContext.Save();
        }

        /// <summary>
        /// ویرایش دسته بندی
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        public void UpdateNewsCategory(NewsCategory entity, NewsCategoryEditCommand command, Guid userId)
        {
            entity.Title = command.Title;
            entity.EnTitle = command.EnTitle;
            entity.OrderId = command.OrderId;
            entity.ModifiedBy = userId;
            entity.ModifiedDate = DateTime.Now;
            _dataContext.NewsCategoryRepository.Update(entity);
            _dataContext.Save();
        }

        /// <summary>
        /// حذف یک دسته بندی بر اساس شناسه
        /// </summary>
        /// <param name="id"></param>
        public void DeleteById(Guid id)
        {
            _dataContext.NewsCategoryRepository.Delete(id);
            _dataContext.Save();
        }

        /// <summary>
        /// حذف یک دسته بندی
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(NewsCategory entity)
        {
            _dataContext.NewsCategoryRepository.Delete(entity);
        }
    }
}
