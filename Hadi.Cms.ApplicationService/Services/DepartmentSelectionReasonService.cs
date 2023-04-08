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
    /// سرویس دلایل انتخاب دپارتمان
    /// </summary>
    public class DepartmentSelectionReasonService
    {
        private readonly DataContext _dataContext;

        public DepartmentSelectionReasonService()
        {
            _dataContext = new DataContext();
        }

        ~DepartmentSelectionReasonService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<IDepartmentSelectionReasonDto> GetList(
            Expression<Func<DepartmentSelectionReason, bool>> filter = null,
            Func<IQueryable<DepartmentSelectionReason>, IOrderedQueryable<DepartmentSelectionReason>> orderBy = null,
            params Expression<Func<DepartmentSelectionReason, object>>[] includes)
        {
            return _dataContext.DepartmentSelectionReasonRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// دریافت بر اساس یک شرط
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IDepartmentSelectionReasonDto Get(Expression<Func<DepartmentSelectionReason, bool>> filter = null,
            params Expression<Func<DepartmentSelectionReason, object>>[] includes)
        {
            return _dataContext.DepartmentSelectionReasonRepository.Get(filter, includes).MapToDto();
        }

        /// <summary>
        /// دریافت بر اساس شناسه
        /// </summary>
        /// <param name="departmentSelectionReasonId"></param>
        /// <returns></returns>
        public IDepartmentSelectionReasonDto Get(Guid departmentSelectionReasonId)
        {
            return _dataContext.DepartmentSelectionReasonRepository.GetByID(departmentSelectionReasonId).MapToDto();
        }

        /// <summary>
        /// ثبت
        /// </summary>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Guid CreateNewDepartmentSelectionReason(DepartmentSelectionReasonCreateCommand command, Guid userId)
        {
            var newDepartmentSelectionReason = new DepartmentSelectionReason
            {
                DepartmentId = command.DepartmentId,
                Title = command.Title,
                Description = command.Description?.Replace("\r\n","<br/>"),
                AttachmentImageId = command.AttachmentImageId,
                IsActive = command.IsActive,
                CreatedBy = userId,
                IsDeleted = false
            };
            Insert(newDepartmentSelectionReason);
            Save();
            return newDepartmentSelectionReason.Id;
        }

        /// <summary>
        /// ثبت
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(DepartmentSelectionReason entity)
        {
            _dataContext.DepartmentSelectionReasonRepository.Insert(entity);
        }

        /// <summary>
        /// ویرایش
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        public void UpdateDepartmentSelectionReason(DepartmentSelectionReason entity,
            DepartmentSelectionReasonEditCommand command, Guid userId)
        {
            entity.Title = command.Title;
            entity.Description = command.Description?.Replace("\r\n", "<br/>");
            entity.AttachmentImageId = command.AttachmentImageId;
            entity.IsActive = command.IsActive;
            entity.ModifiedDate = DateTime.Now;
            entity.ModifiedBy = userId;
            Update(entity);
            Save();
        }

        public void Update(DepartmentSelectionReason entity)
        {
            _dataContext.DepartmentSelectionReasonRepository.Update(entity);
        }

        /// <summary>
        /// حذف
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(DepartmentSelectionReason entity)
        {
            _dataContext.DepartmentSelectionReasonRepository.Delete(entity);
        }

        /// <summary>
        /// حذف بر اساس شناسه
        /// </summary>
        /// <param name="departmentSelectionReasonId"></param>
        public void Delete(Guid departmentSelectionReasonId)
        {
            _dataContext.DepartmentSelectionReasonRepository.Delete(departmentSelectionReasonId);
        }

        /// <summary>
        /// حذف یک محدوده
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRange(IEnumerable<DepartmentSelectionReason> entities)
        {
            _dataContext.DepartmentSelectionReasonRepository.DeleteRange(entities);
        }

        public bool Any(Expression<Func<DepartmentSelectionReason, bool>> filter = null)
        {
            return _dataContext.DepartmentSelectionReasonRepository.Any(filter);
        }

        public int Count(Expression<Func<DepartmentSelectionReason, bool>> filter = null)
        {
            return _dataContext.DepartmentSelectionReasonRepository.Count(filter);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
