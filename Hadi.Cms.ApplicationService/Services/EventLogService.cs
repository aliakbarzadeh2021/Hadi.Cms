using Hadi.Cms.ApplicationService.QueryModels;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Hadi.Cms.ApplicationService.Services
{
    public class EventLogService
    {
        private DataContext _dataContext;

        public EventLogService()
        {
            _dataContext = new DataContext();
        }

        public IEventLogDto Get(Expression<Func<EventLog, bool>> filter)
        {
            var eventLog = _dataContext.EventLogRepository.Get(filter);
            return eventLog.MapToDto();
        }

        public IEventLogDto GetById(object Id)
        {
            var eventLog = _dataContext.EventLogRepository.GetByID(Id);
            return eventLog.MapToDto();
        }

        public List<IEventLogDto> GetList(Expression<Func<EventLog, bool>> filter = null)
        {
            var eventLogs = _dataContext.EventLogRepository.GetList(filter);
            return eventLogs.MapToListDto();
        }

        public void Insert(EventLog model)
        {
            _dataContext.EventLogRepository.Insert(model);
        }

        public void Update(EventLog model)
        {
            _dataContext.EventLogRepository.Update(model);
        }

        public void Delete(EventLog model)
        {
            _dataContext.EventLogRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.EventLogRepository.Delete(Id);
        }

        public void DeleteRange(IEnumerable<EventLog> entities)
        {
            _dataContext.EventLogRepository.DeleteRange(entities);
        }

        public IQueryable<IEventLogDto> Where(Expression<Func<EventLog, bool>> filter)
        {
            var eventLogs = _dataContext.EventLogRepository.Where(filter);
            var eventLogsDto = eventLogs.Select(q => q.MapToDto());
            return eventLogsDto;
        }

        public bool Any(Expression<Func<EventLog, bool>> filter)
        {
            return _dataContext.EventLogRepository.Any(filter);
        }

        public int Count(Expression<Func<EventLog, bool>> where = null)
        {
            return _dataContext.EventLogRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }


        public static IEventLogDto MapEntityToDto(EventLog model)
        {
            var eventLogDto = new EventLogDto
            {
                EventCode = model.EventCode,
                EventDescription = model.EventDescription,
                EventMachineName = model.EventMachineName,
                EventTime = model.EventTime,
                EventType = model.EventType,
                EventUrl = model.EventUrl,
                EventUrlReferrer = model.EventUrlReferrer,
                EventUserAgent = model.EventUserAgent,
                Id = model.Id,
                IpAddress = model.IpAddress,
                Source = model.Source,
                UserId = model.UserId,
                UserName = model.UserName
            };
            return eventLogDto;
        }

    }
}
