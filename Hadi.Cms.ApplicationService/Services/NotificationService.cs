using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Model.QueryModels;
using Hadi.Cms.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hadi.Cms.ApplicationService.CommandModels;

namespace Hadi.Cms.ApplicationService.Services
{
    public class NotificationService
    {
        private DataContext _dataContext;

        public NotificationService()
        {
            _dataContext = new DataContext();
        }


        public INotificationDto Get(Expression<Func<Notification, bool>> filter)
        {
            var Notification = _dataContext.NotificationRepository.Get(filter);
            return Notification.MapToDto();
        }

        public INotificationDto GetById(object Id)
        {
            var Notification = _dataContext.NotificationRepository.GetByID(Id);
            return Notification.MapToDto();
        }

        public List<INotificationDto> GetList(Expression<Func<Notification, bool>> filter = null, Func<IQueryable<Notification>, IOrderedQueryable<Notification>> orderBy = null, params Expression<Func<Notification, object>>[] includes)
        {
            var notifications = _dataContext.NotificationRepository.GetList(filter, orderBy, includes);
            return notifications.MapToListDto();
        }

        public void Insert(Notification model)
        {
            _dataContext.NotificationRepository.Insert(model);
        }

        public void Update(Notification model)
        {
            _dataContext.NotificationRepository.Update(model);
        }

        public void Delete(Notification model)
        {
            _dataContext.NotificationRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.NotificationRepository.Delete(Id);
        }

        public void DeleteRange(IEnumerable<Notification> entities)
        {
            _dataContext.NotificationRepository.DeleteRange(entities);
        }

        public IQueryable<INotificationDto> Where(Expression<Func<Notification, bool>> filter)
        {
            var cities = _dataContext.NotificationRepository.Where(filter);
            var citiesDto = cities.Select(q => q.MapToDto());
            return citiesDto;
        }

        public bool Any(Expression<Func<Notification, bool>> filter)
        {
            return _dataContext.NotificationRepository.Any(filter);
        }

        public int Count(Expression<Func<Notification, bool>> where = null)
        {
            return _dataContext.NotificationRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }


        public List<string> GetTop_N_Notifications(int n)
        {
            var result = new List<string>();
            var notifications = GetList(x => x.IsPublic && !x.IsDelete).OrderByDescending(o => o.SendDateTime).Take(n);

            foreach (var item in notifications)
            {
                if (!item.WithNews)
                {
                    result.Add(item.Text);
                }
                else
                {
                    var newsGuid = (Get(x => x.Id == item.Id))?.Id;
                    result.Add(item.Text + " <a target='_blank' href='/Admin/News/NewsContent?newsId=" + newsGuid + "'>بیشتر</a>");
                }
            }

            return result;
        }

        public bool CreateNewNotification(NotificationCommand model)
        {
            var newNotification = new Notification()
            {
                Text = model.Text,
                SenderUserId = model.Sender.Id,
                ReceiverUserId = model.ReceiverId,
                IsDelete = false,
                IsPublic = model.IsPublic,
                NewsId = model.NewsId,
                ReadDateTime = model.ReceiveTime,
                SendDateTime = model.SendTime,
                Unread = true,
                WithNews = model.WithNews
            };

            Insert(newNotification);
            Save();

            return true;
        }
    }
}
