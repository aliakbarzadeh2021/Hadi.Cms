using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Repository.UnitOfWork;

namespace Hadi.Cms.ApplicationService.Services
{
    public class NlMessageEmailService
    {
        private readonly DataContext _dataContext;

        public NlMessageEmailService()
        {
            _dataContext = new DataContext();
        }

        ~NlMessageEmailService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<INlMessageEmailDto> GetList(Expression<Func<NlMessageEmail, bool>> filter = null,
            Func<IQueryable<NlMessageEmail>, IOrderedQueryable<NlMessageEmail>> orderBy = null,
            params Expression<Func<NlMessageEmail, object>>[] includes)
        {
            return _dataContext.NlMessageEmailRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// حذف بر اساس شناسه
        /// </summary>
        /// <param name="nlMessageEmailId"></param>
        public void Delete(Guid nlMessageEmailId)
        {
            _dataContext.NlMessageEmailRepository.Delete(nlMessageEmailId);
        }


        public void AssignMessageToEmailWithPublishedFalse(Guid messageId, List<Guid> emailIds, Guid userId)
        {
            foreach (var emailId in emailIds)
            {
                var newMessageEmail = new NlMessageEmail
                {
                    NlEmailId = emailId,
                    NlMessageId = messageId,
                    Published = false,
                    CreatedBy = userId
                };
                Insert(newMessageEmail);
            }
            Save();
        }

        public void Insert(NlMessageEmail entity)
        {
            _dataContext.NlMessageEmailRepository.Insert(entity);
        }

        public void Update(NlMessageEmail entity)
        {
            _dataContext.NlMessageEmailRepository.Update(entity);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
