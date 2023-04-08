using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Antlr.Runtime.Tree;
using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Repository.UnitOfWork;

namespace Hadi.Cms.ApplicationService.Services
{
    /// <summary>
    /// سرویس کارمندان
    /// </summary>
    public class EmployeeService
    {
        private readonly DataContext _dataContext;

        public EmployeeService()
        {
            _dataContext = new DataContext();
        }

        ~EmployeeService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست کارمندان
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<IEmployeeDto> GetList(Expression<Func<Employee, bool>> filter = null,
            Func<IQueryable<Employee>, IOrderedQueryable<Employee>> orderBy = null,
            params Expression<Func<Employee, object>>[] includes)
        {
            return _dataContext.EmployeeRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// دریافت یک کارمند بر اساس شرط
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IEmployeeDto Get(Expression<Func<Employee, bool>> filter = null,
            params Expression<Func<Employee, object>>[] includes)
        {
            return _dataContext.EmployeeRepository.Get(filter, includes).MapToDto();
        }

        /// <summary>
        /// دریافت یک کارمند بر اساس شناسه کارمند
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public IEmployeeDto Get(Guid employeeId)
        {
            return _dataContext.EmployeeRepository.GetByID(employeeId).MapToDto();
        }

        /// <summary>
        /// ثبت کارمند جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Guid CreateNewEmployee(EmployeeCreateCommand command, Guid userId)
        {
            var newEmployee = new Employee
            {
                DepartmentId = command.DepartmentId,
                FirstName = command.FirstName,
                LastName = command.LastName,
                RoleTitle = command.RoleTitle,
                OrderId = command.OrderId,
                AttachmentImageId = command.AttachmentImageId,
                CreatedBy = userId,
                IsActive = command.IsActive,
                IsDeleted = false
            };

            Insert(newEmployee);
            Save();
            return newEmployee.Id;
        }

        /// <summary>
        /// ثبت کارمند جدید
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(Employee entity)
        {
            _dataContext.EmployeeRepository.Insert(entity);
        }

        /// <summary>
        /// ویرایش کارمند
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        public void UpdateEmployee(Employee entity, EmployeeEditCommand command, Guid userId)
        {
            entity.DepartmentId = command.DepartmentId;
            entity.FirstName = command.FirstName;
            entity.LastName = command.LastName;
            entity.RoleTitle = command.RoleTitle;
            entity.OrderId = command.OrderId;
            entity.AttachmentImageId = command.AttachmentImageId;
            entity.IsActive = command.IsActive;
            entity.ModifiedBy = userId;
            entity.ModifiedDate = DateTime.Now;
            Update(entity);
            Save();
        }

        /// <summary>
        /// ویرایش کارمند
        /// </summary>
        /// <param name="entity"></param>
        public void Update(Employee entity)
        {
            _dataContext.EmployeeRepository.Update(entity);
        }

        /// <summary>
        /// حذف کارمند
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(Employee entity)
        {
            _dataContext.EmployeeRepository.Delete(entity);
        }

        /// <summary>
        /// حذف کارمند بر اساس شناسه کارمند
        /// </summary>
        /// <param name="employeeId"></param>
        public void Delete(Guid employeeId)
        {
            _dataContext.EmployeeRepository.Delete(employeeId);
        }

        public bool Any(Expression<Func<Employee, bool>> filter = null)
        {
            return _dataContext.EmployeeRepository.Any(filter);
        }

        public int Count(Expression<Func<Employee, bool>> filter = null)
        {
            return _dataContext.EmployeeRepository.Count(filter);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
