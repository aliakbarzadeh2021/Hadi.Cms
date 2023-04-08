using Hadi.Cms.ApplicationService.QueryModels;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Hadi.Cms.ApplicationService.Services
{
    public class ContactUsService
    {

        private DataContext _dataContext;

        public ContactUsService()
        {
            _dataContext = new DataContext();
        }

        public ContactUsDto Get(Expression<Func<ContactUs, bool>> filter = null)
        {
            var contactUs = _dataContext.ContactUsRepository.Get(filter);
            var contactUsDto = MapToDto(contactUs);
            return contactUsDto;
        }

        public ContactUsDto GetById(object Id)
        {
            var contactUs = _dataContext.ContactUsRepository.GetByID(Id);
            var contactUsDto = MapToDto(contactUs);
            return contactUsDto;
        }

        public List<ContactUsDto> GetList(Expression<Func<ContactUs, bool>> filter = null)
        {
            var contactUss = _dataContext.ContactUsRepository.GetList(filter);
            var contactUssDto = contactUss.Select(MapToDto);
            return contactUssDto.ToList();
        }

        public void Insert(ContactUs model)
        {
            _dataContext.ContactUsRepository.Insert(model);
        }

        public void Update(ContactUs model)
        {
            _dataContext.ContactUsRepository.Update(model);
        }

        public void Delete(ContactUs model)
        {
            _dataContext.ContactUsRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.ContactUsRepository.Delete(Id);
        }

        public void DeleteRange(IEnumerable<ContactUs> entities)
        {
            _dataContext.ContactUsRepository.DeleteRange(entities);
        }

        public IQueryable<ContactUs> Where(Expression<Func<ContactUs, bool>> filter)
        {
            return _dataContext.ContactUsRepository.Where(filter);
        }

        public bool Any(Expression<Func<ContactUs, bool>> filter)
        {
            return _dataContext.ContactUsRepository.Any(filter);
        }

        public int Count(Expression<Func<ContactUs, bool>> where = null)
        {
            return _dataContext.ContactUsRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }


        private static ContactUsDto MapToDto(ContactUs model)
        {
            var contactUsDto = new ContactUsDto
            {
                CreatedWhen = model.CreatedWhen,
                Id = model.Id,
                Subject = model.Subject,
                Text = model.Text,
                UserEmail = model.UserEmail,
                UserIp = model.UserIp,
                UserMobile = model.UserMobile,
                UserName = model.UserName
            };
            return contactUsDto;
        }
    }
}
