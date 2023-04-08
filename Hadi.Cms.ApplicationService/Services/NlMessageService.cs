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
    /// سرویس خبرها و پیام های خبرنامه
    /// </summary>
    public class NlMessageService
    {
        private readonly DataContext _dataContext;

        public NlMessageService()
        {
            _dataContext= new DataContext();
        }

        ~NlMessageService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست خبرها بر اساس شرط
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<INlMessageDto> GetList(Expression<Func<NlMessage, bool>> filter = null,
            Func<IQueryable<NlMessage>, IOrderedQueryable<NlMessage>> orderBy = null,
            params Expression<Func<NlMessage, object>>[] includes)
        {
            return _dataContext.NlMessageRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// دریافت یک خبر بر اساس شناسه
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public INlMessageDto Get(Expression<Func<NlMessage, bool>> filter = null,
            params Expression<Func<NlMessage, object>>[] includes)
        {
            return _dataContext.NlMessageRepository.Get(filter, includes).MapToDto();
        }

        /// <summary>
        /// دریافت یک خبر بر اساس
        /// </summary>
        /// <param name="nlMessageId"></param>
        /// <returns></returns>
        public INlMessageDto Get(Guid nlMessageId)
        {
            return _dataContext.NlMessageRepository.GetByID(nlMessageId).MapToDto();
        }

        /// <summary>
        /// ثبت خبر
        /// </summary>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Guid CreateNewMessage(NlMessageCreateCommand command, Guid userId)
        {
            var newNlMessage = new NlMessage
            {
                Title = command.Title,
                Subject = command.Subject,
                Text = command.Text,
                AttachmentId = command.AttachmentId,
                CreatedBy = userId,
                IsActive = true,
                IsDeleted = false
            };
            Insert(newNlMessage);
            Save();

            return newNlMessage.Id;
        }

        /// <summary>
        /// ثبت خبر
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(NlMessage entity)
        {
            _dataContext.NlMessageRepository.Insert(entity);
        }

        /// <summary>
        /// ویرایش خبر
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        public void UpdateMessage(NlMessage entity, NlMessageEditCommand command, Guid userId)
        {
            entity.Title = command.Title;
            entity.Subject = command.Subject;
            entity.Text = command.Text;
            entity.AttachmentId = command.AttachmentId;
            entity.ModifiedBy = userId;
            entity.ModifiedDate = DateTime.Now;
            Update(entity);
            Save();
        }

        /// <summary>
        /// ویرایش خبر
        /// </summary>
        /// <param name="entity"></param>
        public void Update(NlMessage entity)
        {
            _dataContext.NlMessageRepository.Update(entity);
        }

        /// <summary>
        /// حذف خبر
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(NlMessage entity)
        {
            _dataContext.NlMessageRepository.Delete(entity);
        }

        /// <summary>
        /// حذف خبر بر اساس شناسه
        /// </summary>
        /// <param name="nlMessageId"></param>
        public void Delete(Guid nlMessageId)
        {
            _dataContext.NlMessageRepository.Delete(nlMessageId);
        }

        /// <summary>
        /// حذف یک محدوده از اخبار
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRange(IEnumerable<NlMessage> entities)
        {
            _dataContext.NlMessageRepository.DeleteRange(entities);
        }

        public bool Any(Expression<Func<NlMessage, bool>> filter = null)
        {
            return _dataContext.NlMessageRepository.Any(filter);
        }

        public int Count(Expression<Func<NlMessage, bool>> filter = null)
        {
            return _dataContext.NlMessageRepository.Count(filter);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
