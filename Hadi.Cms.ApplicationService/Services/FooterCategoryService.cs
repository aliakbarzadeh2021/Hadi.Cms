using System.Collections.Generic;
using System.Linq.Expressions;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Repository.UnitOfWork;
using System;
using System.Linq;
using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Mappers;

namespace Hadi.Cms.ApplicationService.Services
{
    /// <summary>
    /// سرویس دسته بندی فوتر
    /// </summary>
    public class FooterCategoryService
    {
        private readonly DataContext _dataContext;

        public FooterCategoryService()
        {
            _dataContext = new DataContext();
        }

        ~FooterCategoryService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست دسته بندی های فوتر
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<IFooterCategoryDto> GetList(Expression<Func<FooterCategory, bool>> filter = null,
            Func<IQueryable<FooterCategory>, IOrderedQueryable<FooterCategory>> orderBy = null,
            params Expression<Func<FooterCategory, object>>[] includes)
        {
            return _dataContext.FooterCategory.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// دریافت دسته بندی فوتر بر اساس شناسه
        /// </summary>
        /// <param name="footerCategoryId"></param>
        /// <returns></returns>
        public IFooterCategoryDto Get(Guid footerCategoryId)
        {
            return _dataContext.FooterCategory.GetByID(footerCategoryId).MapToDto();
        }

        /// <summary>
        /// ثبت دسته بندی فوتر
        /// </summary>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Guid AddFooterCategory(FooterCategoryCreateCommand command, Guid userId)
        {
            var newFooterCategory = new FooterCategory
            {
                Title = command.Title,
                Link = command.Link,
                NumberOfShows = command.StatisticalRepresentation ? command.NumberOfShows : 0,
                StatisticalRepresentation = command.StatisticalRepresentation,
                EntityHaveReviewCount = command.StatisticalRepresentation ? command.EntityHaveReviewCount : null,
                OrderId = command.OrderId,
                CreatedBy = userId,
                IsActive = command.IsActive
            };
            Insert(newFooterCategory);
            Save();
            return newFooterCategory.Id;
        }

        /// <summary>
        /// ثبت دسته بندی فوتر
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(FooterCategory entity)
        {
            _dataContext.FooterCategory.Insert(entity);
        }

        /// <summary>
        /// ویرایش دسته بندی
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        public void UpdateFooterCategory(FooterCategory entity, FooterCategoryEditCommand command, Guid userId)
        {
            entity.Title = command.Title;
            entity.Link = command.Link;
            entity.NumberOfShows = command.StatisticalRepresentation ? command.NumberOfShows : 0;
            entity.StatisticalRepresentation = command.StatisticalRepresentation;
            entity.EntityHaveReviewCount = command.StatisticalRepresentation ? command.EntityHaveReviewCount : null;
            entity.OrderId = command.OrderId;
            entity.IsActive = command.IsActive;
            entity.ModifiedBy = userId;
            entity.ModifiedDate = DateTime.Now;
            Update(entity);
            Save();
        }

        /// <summary>
        /// ویرایش دسته بندی فوتر
        /// </summary>
        /// <param name="entity"></param>
        public void Update(FooterCategory entity)
        {
            _dataContext.FooterCategory.Update(entity);
        }

        /// <summary>
        /// حذف دسته بندی
        /// </summary>
        /// <param name="footerCategoryId"></param>
        public void Delete(Guid footerCategoryId)
        {
            _dataContext.FooterCategory.Delete(footerCategoryId);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
