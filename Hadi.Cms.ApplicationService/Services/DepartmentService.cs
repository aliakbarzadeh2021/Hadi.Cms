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
    /// سرویس دپارتمان
    /// </summary>
    public class DepartmentService
    {
        private readonly DataContext _dataContext;

        public DepartmentService()
        {
            _dataContext = new DataContext();
        }

        ~DepartmentService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست دپارتمان ها
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<IDepartmentDto> GetList(Expression<Func<Department, bool>> filter = null,
            Func<IQueryable<Department>, IOrderedQueryable<Department>> orderBy = null,
            params Expression<Func<Department, object>>[] includes)
        {
            return _dataContext.DepartmentRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// دریافت یک دپارتمان بر اساس شرط
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IDepartmentDto Get(Expression<Func<Department, bool>> filter,
            params Expression<Func<Department, object>>[] includes)
        {
            return _dataContext.DepartmentRepository.Get(filter, includes).MapToDto();
        }

        /// <summary>
        /// دریافت یک دپارتمان بر اساس شناسه
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public IDepartmentDto Get(Guid departmentId)
        {
            return _dataContext.DepartmentRepository.GetByID(departmentId).MapToDto();
        }

        /// <summary>
        /// ثبت دپارتمان جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Guid CreateDepartment(DepartmentCreateCommand command, Guid userId)
        {
            var newDepartment = new Department
            {
                Title = command.Title,
                Slogan = command.Slogan,
                Color = command.Color,
                IsActive = command.IsActive,
                AttachmentImageId = command.AttachmentImageId,
                DepartmentBackgroundHeaderAttachmentImageId = command.DepartmentBackgroundHeaderAttachmentImageId,
                JoinOurTeamImageAttachmentId = command.JoinOurTeamImageAttachmentId,
                JoinOurTeamDescription = command.JoinOurTeamDescription,
                StructureAttachmentImageId = command.StructureAttachmentImageId,
                InternshipButtonIsActive = command.InternshipButtonIsActive,
                RecruitmentButtonIsActive = command.RecruitmentButtonIsActive,
                ContactUsLink = string.IsNullOrEmpty(command.ContactUsLink) ? "#" : command.ContactUsLink,
                CreatedBy = userId,
                IsDeleted = false
            };
            Insert(newDepartment);
            Save();
            return newDepartment.Id;
        }

        /// <summary>
        /// ثبت دپارتمان
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(Department entity)
        {
            _dataContext.DepartmentRepository.Insert(entity);
        }

        /// <summary>
        /// ویرایش دپارتمان
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        public void UpdateDepartment(Department entity, DepartmentEditCommand command, Guid userId)
        {
            entity.Title = command.Title;
            entity.Slogan = command.Slogan;
            entity.Color = command.Color;
            entity.AttachmentImageId = command.AttachmentImageId;
            entity.DepartmentBackgroundHeaderAttachmentImageId = command.DepartmentBackgroundHeaderAttachmentImageId;
            entity.JoinOurTeamImageAttachmentId = command.JoinOurTeamImageAttachmentId;
            entity.StructureAttachmentImageId = command.StructureAttachmentImageId;
            entity.InternshipButtonIsActive = command.InternshipButtonIsActive;
            entity.RecruitmentButtonIsActive = command.RecruitmentButtonIsActive;
            entity.JoinOurTeamDescription = command.JoinOurTeamDescription;
            entity.IsActive = command.IsActive;
            entity.ModifiedBy = userId;
            entity.ModifiedDate = DateTime.Now;
            entity.ContactUsLink = string.IsNullOrEmpty(command.ContactUsLink) ? "#" : command.ContactUsLink;
            Update(entity);
            Save();
        }

        /// <summary>
        /// ویرایش دپارتمان
        /// </summary>
        /// <param name="entity"></param>
        public void Update(Department entity)
        {
            _dataContext.DepartmentRepository.Update(entity);
        }

        /// <summary>
        /// حذف دپارتمان
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(Department entity)
        {
            _dataContext.DepartmentRepository.Delete(entity);
        }

        /// <summary>
        /// حذف دپارتمان بر اساس شناسه
        /// </summary>
        /// <param name="departmentId"></param>
        public void Delete(Guid departmentId)
        {
            _dataContext.DepartmentRepository.Delete(departmentId);
        }

        /// <summary>
        /// حذف محدوده ای از دپارتمان ها
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRange(IEnumerable<Department> entities)
        {
            _dataContext.DepartmentRepository.DeleteRange(entities);
        }

        public bool Any(Expression<Func<Department, bool>> filter = null)
        {
            return _dataContext.DepartmentRepository.Any(filter);
        }

        public int Count(Expression<Func<Department, bool>> filter = null)
        {
            return _dataContext.DepartmentRepository.Count(filter);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
