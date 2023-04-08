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
    public class FooterCategoryLinkService
    {
        private readonly DataContext _dataContext;

        public FooterCategoryLinkService()
        {
            _dataContext = new DataContext();
        }

        ~FooterCategoryLinkService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست لینک ها
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<IFooterCategoryLinkDto> GetList(Expression<Func<FooterCategoryLink, bool>> filter = null,
            Func<IQueryable<FooterCategoryLink>, IOrderedQueryable<FooterCategoryLink>> orderBy = null,
            params Expression<Func<FooterCategoryLink, object>>[] includes)
        {
            return _dataContext.FooterCategoryLinkRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// دریافت لینک بر اساس شناسه
        /// </summary>
        /// <param name="footerCategoryLinkId"></param>
        /// <returns></returns>
        public IFooterCategoryLinkDto Get(Guid footerCategoryLinkId)
        {
            return _dataContext.FooterCategoryLinkRepository.GetByID(footerCategoryLinkId).MapToDto();
        }

        /// <summary>
        /// ثبت لینک جدید
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(FooterCategoryLink entity)
        {
            _dataContext.FooterCategoryLinkRepository.Insert(entity);
        }

        /// <summary>
        /// ثبت لینک جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Guid AddFooterCategoryLink(FooterCategoryLinkCreateCommand command, Guid userId)
        {
            var newFooterCategoryLink = new FooterCategoryLink
            {
                FooterCategoryId = command.FooterCategoryId,
                Link = command.Link,
                LinkText = command.LinkText,
                ImageAttachmentGuid = command.ImageAttachmentGuid,
                IsActive = command.IsActive,
                CreatedBy = userId
            };
            Insert(newFooterCategoryLink);
            Save();
            return newFooterCategoryLink.Id;
        }

        /// <summary>
        /// ویرایش لینک
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        public void UpdateFooterCategoryLink(FooterCategoryLink entity, FooterCategoryLinkEditCommand command, Guid userId)
        {
            entity.Link = command.Link;
            entity.LinkText = command.LinkText;
            entity.ImageAttachmentGuid = command.ImageAttachmentGuid;
            entity.IsActive = command.IsActive;
            entity.ModifiedBy = userId;
            entity.ModifiedDate = DateTime.Now;
            Update(entity);
            Save();
        }

        /// <summary>
        /// ویرایش لینک
        /// </summary>
        /// <param name="entity"></param>
        public void Update(FooterCategoryLink entity)
        {
            _dataContext.FooterCategoryLinkRepository.Update(entity);
        }

        /// <summary>
        /// حذف لینک
        /// </summary>
        /// <param name="footerCategoryLinkId"></param>
        public void Delete(Guid footerCategoryLinkId)
        {
            _dataContext.FooterCategoryLinkRepository.Delete(footerCategoryLinkId);
        }

        /// <summary>
        /// حذف محدوده ای از لینک ها
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRange(List<IFooterCategoryLinkDto> entities)
        {
            foreach (var item in entities)
            {
                _dataContext.FooterCategoryLinkRepository.Delete(item.Id);
            }
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
