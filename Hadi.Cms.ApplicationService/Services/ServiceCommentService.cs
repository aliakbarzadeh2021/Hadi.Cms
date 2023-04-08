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
    /// کامنت های خدمت
    /// </summary>
    public class ServiceCommentService
    {
        private readonly DataContext _dataContext;

        public ServiceCommentService()
        {
            _dataContext = new DataContext();
        }

        ~ServiceCommentService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست کامنت های خدمت
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<IServiceCommentDto> GetList(Expression<Func<ServiceComment, bool>> filter = null,
            Func<IQueryable<ServiceComment>, IOrderedQueryable<ServiceComment>> orderBy = null,
            params Expression<Func<ServiceComment, object>>[] includes)
        {
            return _dataContext.ServiceCommentRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// دریافت کامنت خدمت بر اساس شرط
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IServiceCommentDto Get(Expression<Func<ServiceComment, bool>> filter,
            params Expression<Func<ServiceComment, object>>[] includes)
        {
            return _dataContext.ServiceCommentRepository.Get(filter, includes).MapToDto();
        }

        /// <summary>
        /// دریافت کامنت خدمت بر اساس شناسه
        /// </summary>
        /// <param name="serviceCommentId"></param>
        /// <returns></returns>
        public IServiceCommentDto Get(Guid serviceCommentId)
        {
            return _dataContext.ServiceCommentRepository.GetByID(serviceCommentId).MapToDto();
        }

        /// <summary>
        /// ثبت کامنت جدید
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public Guid CreateNewServiceComment(ServiceCommentCreateCommand command , Guid userId)
        {
            var newServiceComment = new ServiceComment()
            {
                ServiceId = command.ServiceId,
                PersonFullName = command.PersonFullName,
                PersonRoleTitle = command.PersonRoleTitle,
                Text = command.Text,
                AttachmentImageId = command.AttachmentImageId,
                CreatedBy = userId
            };
            Insert(newServiceComment);
            Save();
            return newServiceComment.Id;
        }

        /// <summary>
        /// ثبت کامنت برای خدمت
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(ServiceComment entity)
        {
            _dataContext.ServiceCommentRepository.Insert(entity);
        }

        /// <summary>
        /// ویرایش کامنت خدمت
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        public void UpdateServiceComment(ServiceComment entity, ServiceCommentEditCommand command, Guid userId)
        {
            entity.PersonFullName = command.PersonFullName;
            entity.PersonRoleTitle = command.PersonRoleTitle;
            entity.AttachmentImageId = command.AttachmentImageId;
            entity.Text = command.Text;
            entity.ModifiedBy = userId;
            entity.ModifiedDate = DateTime.Now;
            Update(entity);
            Save();
        }

        /// <summary>
        /// ویرایش کامنت خدمت
        /// </summary>
        /// <param name="entity"></param>
        public void Update(ServiceComment entity)
        {
            _dataContext.ServiceCommentRepository.Update(entity);
        }

        /// <summary>
        /// حذف کامنت
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(ServiceComment entity)
        {
            _dataContext.ServiceCommentRepository.Delete(entity);
        }

        /// <summary>
        /// حذف کامنت
        /// </summary>
        /// <param name="serviceCommentId"></param>
        public void Delete(Guid serviceCommentId)
        {
            _dataContext.ServiceCommentRepository.Delete(serviceCommentId);
        }

        /// <summary>
        /// حذف محدوده ای از کامنت ها
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRange(IEnumerable<ServiceComment> entities)
        {
            _dataContext.ServiceCommentRepository.DeleteRange(entities);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
