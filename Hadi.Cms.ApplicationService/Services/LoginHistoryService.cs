using Hadi.Cms.ApplicationService.QueryModels;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Hadi.Cms.ApplicationService.Services
{
    public class LoginHistoryService
    {
        private DataContext _dataContext;

        public LoginHistoryService()
        {
            _dataContext = new DataContext();
        }

        public LoginHistoryDto Get(Expression<Func<LoginHistory, bool>> filter)
        {
            var loginHistory = _dataContext.LoginHistoryRepository.Get(filter);
            var loginHistoryDto = MapToDto(loginHistory);
            return loginHistoryDto;
        }

        public LoginHistoryDto GetById(object Id)
        {
            var loginHistory = _dataContext.LoginHistoryRepository.GetByID(Id);
            var loginHistoryDto = MapToDto(loginHistory);
            return loginHistoryDto;
        }

        public List<LoginHistoryDto> GetList(Expression<Func<LoginHistory, bool>> filter = null)
        {
            var loginHistorys = _dataContext.LoginHistoryRepository.GetList(filter);
            var loginHistorysDto = loginHistorys.Select(MapToDto);
            return loginHistorysDto.ToList();
        }

        public void Insert(LoginHistory model)
        {
            _dataContext.LoginHistoryRepository.Insert(model);
        }

        public void Update(LoginHistory model)
        {
            _dataContext.LoginHistoryRepository.Update(model);
        }

        public void Delete(LoginHistory model)
        {
            _dataContext.LoginHistoryRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.LoginHistoryRepository.Delete(Id);
        }

        public void DeleteRange(IEnumerable<LoginHistory> entities)
        {
            _dataContext.LoginHistoryRepository.DeleteRange(entities);
        }

        public IQueryable<LoginHistory> Where(Expression<Func<LoginHistory, bool>> filter)
        {
            return _dataContext.LoginHistoryRepository.Where(filter);
        }

        public bool Any(Expression<Func<LoginHistory, bool>> filter)
        {
            return _dataContext.LoginHistoryRepository.Any(filter);
        }

        public int Count(Expression<Func<LoginHistory, bool>> where = null)
        {
            return _dataContext.LoginHistoryRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }



        public static LoginHistoryDto MapToDto(LoginHistory model)
        {
            var loginHistoryDto = new LoginHistoryDto
            {
                CreateDate = model.CreateDate,
                Id = model.Id,
                IsLogin = model.IsLogin,
                UserId = model.UserId
            };
            return loginHistoryDto;
        }
    }
}
