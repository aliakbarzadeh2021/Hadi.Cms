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
    /// سرویس کارفرما
    /// </summary>
    public class EmployerService
    {
        private readonly DataContext _dataContext;
        public EmployerService()
        {
            _dataContext = new DataContext();
        }

        /// <summary>
        /// دریافت لیست کارفرمایان
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<IEmployerDto> GetList(Expression<Func<Employer, bool>> filter = null,
            Func<IQueryable<Employer>, IOrderedQueryable<Employer>> orderBy = null,
            params Expression<Func<Employer, object>>[] includes)
        {
            return _dataContext.EmployerRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// دریافت یک کارفرما بر اساس یک شرط
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IEmployerDto Get(Expression<Func<Employer, bool>> filter,
            params Expression<Func<Employer, object>>[] includes)
        {
            return _dataContext.EmployerRepository.Get(filter, includes).MapToDto();
        }

        /// <summary>
        /// دریافت یک کارفرما بر اساس شناسه
        /// </summary>
        /// <param name="employerId"></param>
        /// <returns></returns>
        public IEmployerDto Get(Guid employerId)
        {
            return _dataContext.EmployerRepository.GetByID(employerId).MapToDto();
        }

        /// <summary>
        /// ثبت کارفرمای جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Guid CreateNewEmployer(EmployerCreateCommand command, Guid userId)
        {
            var newEmployer = new Employer
            {
                Name = command.Name,
                LogoGuid = command.LogoGuid,
                Phone = command.Phone,
                Address = command.Address,
                CEOName = command.CEOName,
                CreatedBy = userId,
                IsActive = command.IsActive,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
            };
            Insert(newEmployer);
            Save();
            return newEmployer.Id;
        }

        /// <summary>
        /// ثبت کارفرمای جدید
        /// </summary>
        /// <param name="model"></param>
        public void Insert(Employer model)
        {
            _dataContext.EmployerRepository.Insert(model);
        }

        /// <summary>
        /// ویرایش اطلاعات کارفرما
        /// </summary>
        /// <param name="model"></param>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateEmployer(Employer model, EmployerEditCommand command, Guid userId)
        {
            try
            {
                model.Name = command.Name;
                model.CEOName = command.CEOName;
                model.Phone = command.Phone;
                model.Address = command.Address;
                model.LogoGuid = command.LogoGuid;
                model.IsActive = command.IsActive;
                model.ModifiedDate = DateTime.Now;;
                model.ModifiedBy = userId;
                Update(model);
                Save();
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// ویرایش اطلاعات کارفرما
        /// </summary>
        /// <param name="model"></param>
        public void Update(Employer model)
        {
            _dataContext.EmployerRepository.Update(model);
        }

        /// <summary>
        /// حذف کارفرما
        /// </summary>
        /// <param name="model"></param>
        public void Delete(Employer model)
        {
            _dataContext.EmployerRepository.Delete(model);
        }

        /// <summary>
        /// حذف کارفرما بر اساس شناسه
        /// </summary>
        /// <param name="employerId"></param>
        public void Delete(Guid employerId)
        {
            _dataContext.EmployerRepository.Delete(employerId);
        }

        /// <summary>
        /// حذف یک بازه از کارفرمایان
        /// </summary>
        /// <param name="models"></param>
        public void DeleteRange(List<Employer> models)
        {
            _dataContext.EmployerRepository.DeleteRange(models);
        }

        public bool Any(Expression<Func<Employer, bool>> filter = null)
        {
            return _dataContext.EmployerRepository.Any(filter);
        }

        public int Count(Expression<Func<Employer, bool>> filter = null)
        {
            return _dataContext.EmployerRepository.Count(filter);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
