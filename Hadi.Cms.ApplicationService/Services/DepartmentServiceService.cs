using System.Collections.Generic;
using System.Linq.Expressions;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Repository.UnitOfWork;
using System;
using System.Linq;
using Hadi.Cms.Model.Mappings.Mappers;

namespace Hadi.Cms.ApplicationService.Services
{
    /// <summary>
    /// سرویس خدمات دپارتمان
    /// </summary>
    public class DepartmentServiceService
    {
        private readonly DataContext _dataContext;

        public DepartmentServiceService()
        {
            _dataContext = new DataContext();
        }

        ~DepartmentServiceService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست خدمات دپارتمان
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<IDepartmentServiceDto> GetList(Expression<Func<Model.Entities.DepartmentService, bool>> filter = null,
            Func<IQueryable<Model.Entities.DepartmentService>, IOrderedQueryable<Model.Entities.DepartmentService>> orderBy = null,
            params Expression<Func<Model.Entities.DepartmentService, object>>[] includes)
        {
            return _dataContext.DepartmentServiceRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// نسبت دادن خدمت به دپارتمان
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="servicesId"></param>
        /// <param name="userId"></param>
        public void AssignServiceToDepartment(Guid departmentId, List<Guid> servicesId, Guid userId)
        {
            #region Remove old services

            var departmentServices = GetList(d => d.DepartmentId == departmentId);
            foreach (var departmentService in departmentServices)
            {
                Delete(departmentService.Id);
            }
            #endregion

            if (servicesId != null)
            {
                foreach (var serviceId in servicesId)
                {
                    var newDepartmentService = new Model.Entities.DepartmentService
                    {
                        DepartmentId = departmentId,
                        ServiceId = serviceId,
                        CreatedBy = userId,
                        IsActive = true,
                        IsDeleted = false
                    };
                    Insert(newDepartmentService);
                }
            }
            Save();
        }


        /// <summary>
        /// ثبت خدمت برای دپارتمان
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(Model.Entities.DepartmentService entity)
        {
            _dataContext.DepartmentServiceRepository.Insert(entity);
        }

        /// <summary>
        /// حذف سرویس از دپارتمان
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(Model.Entities.DepartmentService entity)
        {
            _dataContext.DepartmentServiceRepository.Delete(entity);
        }

        /// <summary>
        /// حذف سرویس از دپارتمان بر اساس شناسه
        /// </summary>
        /// <param name="departmentServiceId"></param>
        public void Delete(Guid departmentServiceId)
        {
            _dataContext.DepartmentServiceRepository.Delete(departmentServiceId);
        }

        /// <summary>
        /// حذف محدوده ای از سرویس ها از دپارتمان
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRange(IEnumerable<Model.Entities.DepartmentService> entities)
        {
            _dataContext.DepartmentServiceRepository.DeleteRange(entities);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
