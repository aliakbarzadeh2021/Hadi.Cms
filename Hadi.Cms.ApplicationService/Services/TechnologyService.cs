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
    /// سرویس تکنولوژِی
    /// </summary>
    public class TechnologyService
    {
        private readonly DataContext _dataContext;

        public TechnologyService()
        {
            _dataContext = new DataContext();
        }

        ~TechnologyService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست تکنولوژی ها
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<ITechnologyDto> GetList(Expression<Func<Technology, bool>> filter = null,
            Func<IQueryable<Technology>, IOrderedQueryable<Technology>> orderBy = null,
            params Expression<Func<Technology, object>>[] includes)
        {
            return _dataContext.TechnologyRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// دریافت یک تکنولوژی بر اساس فیلتر ورودی
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public ITechnologyDto Get(Expression<Func<Technology, bool>> filter,
            params Expression<Func<Technology, object>>[] includes)
        {
            return _dataContext.TechnologyRepository.Get(filter, includes).MapToDto();
        }

        /// <summary>
        /// دریافت یک تکنولوژی بر اساس شرط
        /// </summary>
        /// <param name="technologyId"></param>
        /// <returns></returns>
        public ITechnologyDto Get(Guid technologyId)
        {
            return _dataContext.TechnologyRepository.GetByID(technologyId).MapToDto();
        }

        /// <summary>
        /// ثبت تکنولوژِ جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Guid CreateNewTechnology(TechnologyCreateCommand command, Guid userId)
        {
            var newTechnology = new Technology
            {
                Name = command.Name,
                ImageGuid = command.ImageGuid,
                IsTool = command.IsTool,
                IsActive = command.IsActive,
                IsDeleted = false,
                CreatedBy = userId
            };

            Insert(newTechnology);
            Save();
            return newTechnology.Id;
        }

        /// <summary>
        /// ثبت تکنولوژی جدید
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(Technology entity)
        {
            _dataContext.TechnologyRepository.Insert(entity);
        }

        /// <summary>
        /// ویرایش تکنولوِژی
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateTechnology(Technology entity, TechnologyEditCommand command, Guid userId)
        {
            try
            {
                entity.Name = command.Name;
                entity.ImageGuid = command.ImageGuid;
                entity.IsTool = command.IsTool;
                entity.IsActive = command.IsActive;
                entity.ModifiedBy = userId;
                entity.ModifiedDate = DateTime.Now;
                Update(entity);
                Save();
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// ویرایش یک تکنولوژی
        /// </summary>
        /// <param name="entity"></param>
        public void Update(Technology entity)
        {
            _dataContext.TechnologyRepository.Update(entity);
        }

        /// <summary>
        /// حذف تکنولوژی
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(Technology entity)
        {
            _dataContext.TechnologyRepository.Delete(entity);
        }

        /// <summary>
        /// حذف تکنولوژی
        /// </summary>
        /// <param name="technologyId"></param>
        public void Delete(Guid technologyId)
        {
            _dataContext.TechnologyRepository.Delete(technologyId);
        }

        public int Count(Expression<Func<Technology, bool>> filter = null)
        {
            return _dataContext.TechnologyRepository.Count(filter);
        }

        public bool Any(Expression<Func<Technology, bool>> filter = null)
        {
            return _dataContext.TechnologyRepository.Any(filter);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
