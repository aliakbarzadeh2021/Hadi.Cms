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
    /// سرویس رویداد
    /// </summary>
    public class EventService
    {
        private readonly DataContext _dataContext;

        public EventService()
        {
            _dataContext = new DataContext();
        }

        ~EventService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست رویداد ها
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<IEventDto> GetList(Expression<Func<Event, bool>> filter = null,
            Func<IQueryable<Event>, IOrderedQueryable<Event>> orderBy = null,
            params Expression<Func<Event, object>>[] includes)
        {
            return _dataContext.EventRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// دریافت یک رویداد بر اساس شناسه
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public IEventDto Get(Guid eventId)
        {
            return _dataContext.EventRepository.GetByID(eventId).MapToDto();
        }

        /// <summary>
        /// دریافت یک رویداد بر اساس شرط
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IEventDto Get(Expression<Func<Event, bool>> filter = null,
            params Expression<Func<Event, object>>[] includes)
        {
            return _dataContext.EventRepository.Get(filter, includes).MapToDto();
        }

        /// <summary>
        /// ثبت رویداد جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Guid CreateNewEvent(EventCreateCommand command, Guid userId)
        {
            var newEvent = new Event
            {
                Title = command.Title,
                Link = string.IsNullOrEmpty(command.Link) ? "#" : command.Link,
                AttachmentImageId = command.AttachmentImageId,
                CreatedBy = userId,
                IsActive = command.IsActive
            };
            Insert(newEvent);
            Save();
            return newEvent.Id;
        }

        /// <summary>
        /// ثبت رویداد جدید
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(Event entity)
        {
            _dataContext.EventRepository.Insert(entity);
        }

        /// <summary>
        /// ویرایش رویداد
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        public void UpdateEvent(Event entity, EventEditCommand command, Guid userId)
        {
            entity.Title = command.Title;
            entity.Link = string.IsNullOrEmpty(command.Link) ? "#" : command.Link;
            entity.AttachmentImageId = command.AttachmentImageId;
            entity.IsActive = command.IsActive;
            entity.ModifiedBy = userId;
            entity.ModifiedDate = DateTime.Now;
            Update(entity);
            Save();
        }

        /// <summary>
        /// ویرایش رویداد
        /// </summary>
        /// <param name="entity"></param>
        public void Update(Event entity)
        {
            _dataContext.EventRepository.Update(entity);
        }

        /// <summary>
        /// حذف رویداد
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(Event entity)
        {
            _dataContext.EventRepository.Delete(entity);
        }

        /// <summary>
        /// حذف رویداد بر اساس شناسه
        /// </summary>
        /// <param name="eventId"></param>
        public void Delete(Guid eventId)
        {
            _dataContext.EventRepository.Delete(eventId);
        }

        /// <summary>
        /// حذف محدوده ای از رویداد ها
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRange(IEnumerable<Event> entities)
        {
            _dataContext.EventRepository.DeleteRange(entities);
        }

        public bool Any(Expression<Func<Event, bool>> filter = null)
        {
            return _dataContext.EventRepository.Any(filter);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
