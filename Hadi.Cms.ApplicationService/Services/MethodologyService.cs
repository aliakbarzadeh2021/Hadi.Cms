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
    /// سرویس متدولوژی دپارتمان ها
    /// </summary>
    public class MethodologyService
    {
        private readonly DataContext _dataContext;

        public MethodologyService()
        {
            _dataContext = new DataContext();
        }

        ~MethodologyService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست متدلوژی ها
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<IMethodologyDto> GetList(Expression<Func<Methodology, bool>> filter = null,
            Func<IQueryable<Methodology>, IOrderedQueryable<Methodology>> orderBy = null,
            params Expression<Func<Methodology, object>>[] includes)
        {
            return _dataContext.MethodologyRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// دریافت یک متدولوژِی بر اساس شرط
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IMethodologyDto Get(Expression<Func<Methodology, bool>> filter = null,
            params Expression<Func<Methodology, object>>[] includes)
        {
            return _dataContext.MethodologyRepository.Get(filter, includes).MapToDto();
        }

        /// <summary>
        /// دریافت یک متدولوژی بر اساس شناسه
        /// </summary>
        /// <param name="methodologyId"></param>
        /// <returns></returns>
        public IMethodologyDto Get(Guid methodologyId)
        {
            return _dataContext.MethodologyRepository.GetByID(methodologyId).MapToDto();
        }

        /// <summary>
        /// ثبت متدولوژی جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Guid CreateNewTechnology(MethodologyCreateCommand command, Guid userId)
        {
            var newMethodology = new Methodology
            {
                DepartmentId = command.DepartmentId,
                Title = command.Title,
                Description = command.Description,
                ImageAttachmentId = command.ImageAttachmentId,
                IsActive = command.IsActive,
                CreatedBy = userId,
                IsDeleted = false
            };

            Insert(newMethodology);
            Save();
            return newMethodology.Id;
        }

        /// <summary>
        /// ثبت متدلوژی جدید
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(Methodology entity)
        {
            _dataContext.MethodologyRepository.Insert(entity);
        }

        /// <summary>
        /// ویرایش متدلوزژی
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        public void UpdateMethodology(Methodology entity, MethodologyEditCommand command, Guid userId)
        {
            entity.DepartmentId = command.DepartmentId;
            entity.Title = command.Title;
            entity.Description = command.Description;
            entity.ImageAttachmentId = command.ImageAttachmentId;
            entity.IsActive = command.IsActive;
            entity.ModifiedBy = userId;
            entity.ModifiedDate = DateTime.Now;
            Update(entity);
            Save();
        }

        /// <summary>
        /// ویرایش متدولوژِی
        /// </summary>
        /// <param name="entity"></param>
        public void Update(Methodology entity)
        {
            _dataContext.MethodologyRepository.Update(entity);
        }

        /// <summary>
        /// حذف متدولوژی
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(Methodology entity)
        {
            _dataContext.MethodologyRepository.Delete(entity);
        }

        /// <summary>
        /// حذف تکنولوژی بر اساس شرط
        /// </summary>
        /// <param name="methodologyId"></param>
        public void Delete(Guid methodologyId)
        {
            _dataContext.MethodologyRepository.Delete(methodologyId);
        }

        /// <summary>
        /// حذف یک محدوده از متدولوژی ها
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRange(IEnumerable<Methodology> entities)
        {
            _dataContext.MethodologyRepository.DeleteRange(entities);
        }

        public bool Any(Expression<Func<Methodology, bool>> filter = null)
        {
            return _dataContext.MethodologyRepository.Any(filter);
        }

        public int Count(Expression<Func<Methodology, bool>> filter = null)
        {
            return _dataContext.MethodologyRepository.Count(filter);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
