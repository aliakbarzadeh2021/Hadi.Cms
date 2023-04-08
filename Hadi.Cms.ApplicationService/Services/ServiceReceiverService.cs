using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
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
    public class ServiceReceiverService
    {
        private readonly DataContext _dataContext;

        public ServiceReceiverService()
        {
            _dataContext = new DataContext();
        }

        ~ServiceReceiverService()
        {
            _dataContext?.Dispose();
        }


        /// <summary>
        /// دریافت لیست دریافت کنندگان خدمت
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<IServiceReceiverDto> GetList(Expression<Func<ServiceReceiver, bool>> filter = null,
            Func<IQueryable<ServiceReceiver>, IOrderedQueryable<ServiceReceiver>> orderBy = null,
            params Expression<Func<ServiceReceiver, object>>[] includes)
        {
            return _dataContext.ServiceReceiverRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// دریافت اطلاعات دریافت کننده خدمت
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IServiceReceiverDto Get(Expression<Func<ServiceReceiver, bool>> filter,
            params Expression<Func<ServiceReceiver, object>>[] includes)
        {
            return _dataContext.ServiceReceiverRepository.Get(filter, includes).MapToDto();
        }

        /// <summary>
        /// دریافت اطلاعات دریافت کننده خدمت بر اساس شناسه
        /// </summary>
        /// <param name="serviceReceiverId"></param>
        /// <returns></returns>
        public IServiceReceiverDto Get(Guid serviceReceiverId)
        {
            return _dataContext.ServiceReceiverRepository.GetByID(serviceReceiverId).MapToDto();
        }

        /// <summary>
        /// ثبت
        /// </summary>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Guid CreateNewServiceReceiver(ServiceReceiverCreateCommand command, Guid userId)
        {
            var newServiceReceiver = new ServiceReceiver()
            {
                ReceiverName = command.ReceiverName,
                AttachmentImageId = command.AttachmentImageId,
                CreatedBy = userId
            };
            Insert(newServiceReceiver);
            Save();
            return newServiceReceiver.Id;
        }

        /// <summary>
        /// ثبت
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(ServiceReceiver entity)
        {
            _dataContext.ServiceReceiverRepository.Insert(entity);
        }

        /// <summary>
        /// ویرایش
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        public void UpdateServiceReceiver(ServiceReceiver entity, ServiceReceiverEditCommand command, Guid userId)
        {
            entity.ReceiverName = command.ReceiverName;
            entity.AttachmentImageId = command.AttachmentImageId;
            entity.ModifiedBy = userId;
            entity.ModifiedDate = DateTime.Now;
            Update(entity);
            Save();
        }

        /// <summary>
        /// ویرایش
        /// </summary>
        /// <param name="entity"></param>
        public void Update(ServiceReceiver entity)
        {
            _dataContext.ServiceReceiverRepository.Update(entity);
        }

        /// <summary>
        /// حذف
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(ServiceReceiver entity)
        {
            _dataContext.ServiceReceiverRepository.Delete(entity);
        }

        /// <summary>
        /// حذف بر اساس شناسه
        /// </summary>
        /// <param name="serviceReceiverId"></param>
        public void Delete(Guid serviceReceiverId)
        {
            _dataContext.ServiceReceiverRepository.Delete(serviceReceiverId);
        }

        public void DeleteRange(IEnumerable<ServiceReceiver> entities)
        {
            _dataContext.ServiceReceiverRepository.DeleteRange(entities);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}