using System;
using System.Collections.Generic;
using System.Linq;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Repository.UnitOfWork;
using System.Linq.Expressions;
using Hadi.Cms.Model.Mappings.Mappers;

namespace Hadi.Cms.ApplicationService.Services
{
    /// <summary>
    /// برچسب های سرویس
    /// </summary>
    public class ServiceTagService
    {
        private readonly DataContext _dataContext;

        public ServiceTagService()
        {
            _dataContext  =new DataContext();
        }

        ~ServiceTagService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست تگ های خدمت
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<IServiceTagDto> GetList(Expression<Func<ServiceTag, bool>> filter = null,
            Func<IQueryable<ServiceTag>, IOrderedQueryable<ServiceTag>> orderBy = null,
            params Expression<Func<ServiceTag, object>>[] includes)
        {
            return _dataContext.ServiceTagRepository.GetList(filter, orderBy, includes).MapToListDto();
        }


        /// <summary>
        /// نسبت دادن تگ به خدمت
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="serviceTagsId"></param>
        /// <param name="userId"></param>
        public void AssignTagsToService(Guid serviceId, List<Guid> serviceTagsId, Guid userId)
        {
            var oldServiceTags = GetList(s => s.ServiceId == serviceId);
            foreach (var serviceTagId in oldServiceTags)
            {
                Delete(serviceTagId.Id);
            }

            if(serviceTagsId != null && serviceTagsId.Count > 0)
            {
                foreach (var newServiceTagId in serviceTagsId)
                {
                    var newServiceTag = new ServiceTag
                    {
                        ServiceId = serviceId,
                        TagId = newServiceTagId,
                        CreatedBy = userId
                    };
                    Insert(newServiceTag);
                }
            }
            Save();
        }

        /// <summary>
        /// ثبت تگ خدمت
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(ServiceTag entity)
        {
            _dataContext.ServiceTagRepository.Insert(entity);
        }


        /// <summary>
        /// حذف تگ از خدمت
        /// </summary>
        /// <param name="serviceTagId"></param>
        public void Delete(Guid serviceTagId)
        {
            _dataContext.ServiceTagRepository.Delete(serviceTagId);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
