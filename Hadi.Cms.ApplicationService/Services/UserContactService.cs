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
    public class UserContactService
    {
        private DataContext _dataContext;

        public UserContactService()
        {
            _dataContext = new DataContext();
        }

        public IUserContactDto Get(Expression<Func<UserContact, bool>> filter)
        {
            var userContact = _dataContext.UserContactRepository.Get(filter);
            return userContact.MapToDto();
        }

        public IUserContactDto GetById(object Id)
        {
            var userContact = _dataContext.UserContactRepository.GetByID(Id);
            return userContact.MapToDto();
        }

        public List<IUserContactDto> GetList(Expression<Func<UserContact, bool>> filter = null)
        {
            var userContacts = _dataContext.UserContactRepository.GetList(filter);
            return userContacts.MapToListDto();
        }

        public void Insert(UserContact model)
        {
            _dataContext.UserContactRepository.Insert(model);
        }

        public void Update(UserContact model)
        {
            _dataContext.UserContactRepository.Update(model);
        }

        public void Delete(UserContact model)
        {
            _dataContext.UserContactRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.UserContactRepository.Delete(Id);
        }


        public void DeleteRange(IEnumerable<UserContact> entities)
        {
            _dataContext.UserContactRepository.DeleteRange(entities);
        }

        public IQueryable<IUserContactDto> Where(Expression<Func<UserContact, bool>> filter)
        {
            var userContact = _dataContext.UserContactRepository.Where(filter);
            var userContactDto = userContact.Select(q => q.MapToDto());
            return userContactDto;
        }

        public bool Any(Expression<Func<UserContact, bool>> filter)
        {
            return _dataContext.UserContactRepository.Any(filter);
        }

        public int Count(Expression<Func<UserContact, bool>> where = null)
        {
            return _dataContext.UserContactRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }



        public static UserContactDto MapToDto(UserContact model)
        {
            var userContactDto = new UserContactDto
            {
                ContactUserId = model.ContactUserId,
                CreateDate = model.CreateDate,
                Id = model.Id,
                UserId = model.UserId
            };
            return userContactDto;
        }

    }
}
