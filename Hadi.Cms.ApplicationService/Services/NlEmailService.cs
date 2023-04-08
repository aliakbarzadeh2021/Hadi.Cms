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
    /// سرویس ایمیل های خبرنامه
    /// </summary>
    public class NlEmailService
    {
        private readonly DataContext _dataContext;

        public NlEmailService()
        {
            _dataContext = new DataContext();
        }

        ~NlEmailService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست ایمیل ها
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<INlEmailDto> GetList(Expression<Func<NlEmail, bool>> filter = null,
            Func<IQueryable<NlEmail>, IOrderedQueryable<NlEmail>> orderBy = null,
            params Expression<Func<NlEmail, object>>[] includes)
        {
            return _dataContext.NlEmailRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// دریافت یک ایمیل بر اساس شناسه
        /// </summary>
        /// <param name="nlEmailId"></param>
        /// <returns></returns>
        public INlEmailDto GetById(Guid nlEmailId)
        {
            var nlEmail = _dataContext.NlEmailRepository.GetByID(nlEmailId);
            return nlEmail.MapToDto();
        }

        /// <summary>
        /// ثبت ایمیل برای خبرنامه
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public Guid CreateNewEmail(NlEmailCreateCommand command)
        {
            var newNlEmail = new NlEmail
            {
                Email = command.Email,
                CreatedBy = Guid.Empty,
                IsActive = true,
                IsDeleted = false
            };
            Insert(newNlEmail);
            Save();
            return newNlEmail.Id;
        }

        /// <summary>
        /// ثبت
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(NlEmail entity)
        {
            _dataContext.NlEmailRepository.Insert(entity);
        }

        /// <summary>
        /// ویرایش ایمیل
        /// </summary>
        /// <param name="entity"></param>
        public void Update(NlEmail entity)
        {
            _dataContext.NlEmailRepository.Update(entity);
        }

        /// <summary>
        /// حذف ایمیل
        /// </summary>
        /// <param name="nlEmailId"></param>
        public void Delete(Guid nlEmailId)
        {
            _dataContext.NlEmailRepository.Delete(nlEmailId);
        }

        public bool Any(Expression<Func<NlEmail, bool>> filter = null)
        {
            return _dataContext.NlEmailRepository.Any(filter);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
