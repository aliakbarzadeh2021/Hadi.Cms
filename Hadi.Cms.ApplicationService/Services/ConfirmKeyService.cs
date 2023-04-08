using Hadi.Cms.ApplicationService.QueryModels;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Hadi.Cms.ApplicationService.Services
{
    public class ConfirmKeyService
    {
        private DataContext _dataContext;

        public ConfirmKeyService()
        {
            _dataContext = new DataContext();
        }

        public ConfirmKeyDto Get(Expression<Func<ConfirmKey, bool>> filter = null)
        {
            var confirmKey = _dataContext.ConfirmKeyRepository.Get(filter);
            var confirmKeyDto = MapToDto(confirmKey);
            return confirmKeyDto;
        }

        public ConfirmKeyDto GetById(object Id)
        {
            var confirmKey = _dataContext.ConfirmKeyRepository.GetByID(Id);
            var confirmKeyDto = MapToDto(confirmKey);
            return confirmKeyDto;
        }

        public List<ConfirmKeyDto> GetList(Expression<Func<ConfirmKey, bool>> filter = null)
        {
            var confirmKeys = _dataContext.ConfirmKeyRepository.GetList(filter);
            var confirmKeysDto = confirmKeys.Select(MapToDto);
            return confirmKeysDto.ToList();
        }

        public void Insert(ConfirmKey model)
        {
            _dataContext.ConfirmKeyRepository.Insert(model);
        }

        public void Update(ConfirmKey model)
        {
            _dataContext.ConfirmKeyRepository.Update(model);
        }

        public void Delete(ConfirmKey model)
        {
            _dataContext.ConfirmKeyRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.ConfirmKeyRepository.Delete(Id);
        }

        public void DeleteRange(IEnumerable<ConfirmKey> entities)
        {
            _dataContext.ConfirmKeyRepository.DeleteRange(entities);
        }

        public IQueryable<ConfirmKey> Where(Expression<Func<ConfirmKey, bool>> filter)
        {
            return _dataContext.ConfirmKeyRepository.Where(filter);
        }

        public bool Any(Expression<Func<ConfirmKey, bool>> filter)
        {
            return _dataContext.ConfirmKeyRepository.Any(filter);
        }

        public int Count(Expression<Func<ConfirmKey, bool>> where = null)
        {
            return _dataContext.ConfirmKeyRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }

        public static ConfirmKeyDto MapToDto(ConfirmKey model)
        {
            var confirmKeyDto = new ConfirmKeyDto
            {
                CreateDate = model.CreateDate,
                ExpireDate = model.ExpireDate,
                Id = model.Id,
                IsEmail = model.IsEmail,
                IsSms = model.IsSms,
                LinkGuid = model.LinkGuid,
                SmsKey = model.SmsKey,
                UserEmailAddress = model.UserEmailAddress,
                UserId = model.UserId,
                UserMobileNumber = model.UserMobileNumber
            };
            return confirmKeyDto;
        }

    }
}
