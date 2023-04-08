using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Repository.UnitOfWork;

namespace Hadi.Cms.ApplicationService.Services
{
    /// <summary>
    /// سرویس خدمت
    /// </summary>
    public class ServiceService
    {
        private readonly DataContext _dataContext;

        public ServiceService()
        {
            _dataContext = new DataContext();
        }

        ~ServiceService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست خدمات
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<IServiceDto> GetList(Expression<Func<Service, bool>> filter = null,
            Func<IQueryable<Service>, IOrderedQueryable<Service>> orderBy = null,
            params Expression<Func<Service, object>>[] includes)
        {
            var services = _dataContext.ServiceRepository.GetList(filter, orderBy, includes);
            return services.MapToListDto();
        }

        /// <summary>
        /// دریافت یک خدمت بر اساس شرط
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IServiceDto Get(Expression<Func<Service, bool>> filter,
            params Expression<Func<Service, object>>[] includes)
        {
            return _dataContext.ServiceRepository.Get(filter, includes).MapToDto();
        }

        /// <summary>
        /// دریافت یک خدمت بر اساس شناسه
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public IServiceDto Get(Guid serviceId)
        {
            return _dataContext.ServiceRepository.GetByID(serviceId).MapToDto();
        }

        /// <summary>
        /// ثبت سرویس جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Guid CreateNewService(ServiceCreateCommand command, Guid userId)
        {
            var newService = new Service
            {
                Title = command.Title?.Replace("\r\n","<br/>"),
                Description = command.Description,
                AttachmentImageId = command.AttachmentImageId,
                SectionOneDescription = command.SectionOneDescription,
                SectionOneImageGuid = command.SectionOneImageGuid,
                SectionOneThumbnailImageGuid = command.SectionOneThumbnailImageGuid,
                SectionTwoDescription = command.SectionTwoDescription,
                SectionTwoImageGuid = command.SectionTwoImageGuid,
                SectionTwoThumbnailImageGuid = command.SectionTwoThumbnailImageGuid,
                SectionThreeDescription = command.SectionThreeDescription,
                SectionThreeImageGuid = command.SectionThreeImageGuid,
                SectionThreeThumbnailImageGuid = command.SectionThreeThumbnailImageGuid,
                SectionFourDescription = command.SectionFourDescription,
                SectionFourImageGuid = command.SectionFourImageGuid,
                SectionFourThumbnailImageGuid = command.SectionFourThumbnailImageGuid,
                ServiceLogoId = command.ServiceLogoId,
                ReviewCount = 0,
                IsActive = command.IsActive,
                CreatedBy = userId,
                IsDeleted = false
            };
            Insert(newService);
            Save();
            return newService.Id;
        }

        /// <summary>
        /// ثبت خدمت جدید
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(Service entity)
        {
            _dataContext.ServiceRepository.Insert(entity);
        }

        /// <summary>
        /// ویرایش خدمت
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        public void UpdateService(Service entity, ServiceEditCommand command, Guid userId)
        {
            entity.Title = command.Title?.Replace("\r\n", "<br/>");
            entity.Description = command.Description;
            entity.AttachmentImageId = command.AttachmentImageId;
            entity.SectionOneDescription = command.SectionOneDescription;
            entity.SectionOneImageGuid = command.SectionOneImageGuid;
            entity.SectionOneThumbnailImageGuid = command.SectionOneThumbnailImageGuid;
            entity.SectionTwoDescription = command.SectionTwoDescription;
            entity.SectionTwoImageGuid = command.SectionTwoImageGuid;
            entity.SectionTwoThumbnailImageGuid = command.SectionTwoThumbnailImageGuid;
            entity.SectionThreeDescription = command.SectionThreeDescription;
            entity.SectionThreeImageGuid = command.SectionThreeImageGuid;
            entity.SectionThreeThumbnailImageGuid = command.SectionThreeThumbnailImageGuid;
            entity.SectionFourDescription = command.SectionFourDescription;
            entity.SectionFourImageGuid = command.SectionFourImageGuid;
            entity.SectionFourThumbnailImageGuid = command.SectionFourThumbnailImageGuid;
            entity.ServiceLogoId = command.ServiceLogoId;
            entity.IsActive = command.IsActive;
            entity.ModifiedBy = userId;
            entity.ModifiedDate = DateTime.Now;
            Update(entity);
            Save();
        }

        /// <summary>
        /// ویرایش خدمت
        /// </summary>
        /// <param name="entity"></param>
        public void Update(Service entity)
        {
            _dataContext.ServiceRepository.Update(entity);
        }

        /// <summary>
        /// حذف خدمت
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(Service entity)
        {
            _dataContext.ServiceRepository.Delete(entity);
        }

        /// <summary>
        /// حذف بر اساس شناسه
        /// </summary>
        /// <param name="serviceId"></param>
        public void Delete(Guid serviceId)
        {
            _dataContext.ServiceRepository.Delete(serviceId);
        }

        /// <summary>
        /// حذف محدوده ای از خدمات
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRange(IEnumerable<Service> entities)
        {
            _dataContext.ServiceRepository.DeleteRange(entities);
        }

        public bool Any(Expression<Func<Service, bool>> filter = null)
        {
            return _dataContext.ServiceRepository.Any(filter);
        }

        public int Count(Expression<Func<Service, bool>> filter = null)
        {
            return _dataContext.ServiceRepository.Count(filter);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
