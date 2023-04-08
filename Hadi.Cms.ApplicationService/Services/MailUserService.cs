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
    public class MailUserService
    {
        private DataContext _dataContext;

        public MailUserService()
        {
            _dataContext = new DataContext();
        }

        public IMailUserDto Get(Expression<Func<MailUser, bool>> filter)
        {
            var mailUser = _dataContext.MailUserRepository.Get(filter);
            return mailUser.MapToDto();
        }

        public IMailUserDto GetById(object Id)
        {
            var mailUser = _dataContext.MailUserRepository.GetByID(Id);
            return mailUser.MapToDto();
        }

        public List<IMailUserDto> GetList(Expression<Func<MailUser, bool>> filter = null)
        {
            var mailUsers = _dataContext.MailUserRepository.GetList(filter);
            return mailUsers.MapToListDto();
        }

        public void Insert(MailUser model)
        {
            _dataContext.MailUserRepository.Insert(model);
        }

        public void Update(MailUser model)
        {
            _dataContext.MailUserRepository.Update(model);
        }

        public void Delete(MailUser model)
        {
            _dataContext.MailUserRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.MailUserRepository.Delete(Id);
        }

        public void DeleteRange(IEnumerable<MailUser> entities)
        {
            _dataContext.MailUserRepository.DeleteRange(entities);
        }


        public IQueryable<IMailUserDto> Where(Expression<Func<MailUser, bool>> filter)
        {
            var mailUsers = _dataContext.MailUserRepository.Where(filter);
            var mailUsersDto = mailUsers.Select(q => q.MapToDto());
            return mailUsersDto;
        }

        public bool Any(Expression<Func<MailUser, bool>> filter)
        {
            return _dataContext.MailUserRepository.Any(filter);
        }

        public int Count(Expression<Func<MailUser, bool>> where = null)
        {
            return _dataContext.MailUserRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }


        public static MailUserDto MapToDto(MailUser model)
        {
            var mailUserDto = new MailUserDto
            {
                DeletedByReceiver = model.DeletedByReceiver,
                DeletedBySender = model.DeletedBySender,
                Id = model.Id,
                MailId = model.MailId,
                ReceiverUserId = model.ReceiverUserId,
                SendDateTime = model.SendDateTime,
                SenderUserId = model.SenderUserId,
                Unread = model.Unread
            };
            return mailUserDto;
        }


    }
}
