using Hadi.Cms.Repository.UnitOfWork;
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

namespace Hadi.Cms.ApplicationService.Services
{
    /// <summary>
    /// سرویس رزومه
    /// </summary>
    public class ResumeService
    {
        private readonly DataContext _dataContext;

        public ResumeService()
        {
            _dataContext = new DataContext();
        }

        ~ResumeService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست رزومه ها
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<IResumeDto> GetList(Expression<Func<Resume, bool>> filter = null,
            Func<IQueryable<Resume>, IOrderedQueryable<Resume>> orderBy = null,
            params Expression<Func<Resume, object>>[] includes)
        {
            return _dataContext.ResumeRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// دریافت یک رزومه بر اساس شناسه رزومه
        /// </summary>
        /// <param name="resumeId"></param>
        /// <returns></returns>
        public IResumeDto Get(Guid resumeId)
        {
            return _dataContext.ResumeRepository.GetByID(resumeId).MapToDto();
        }

        /// <summary>
        /// ثبت رزومه جدید
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(Resume entity)
        {
            _dataContext.ResumeRepository.Insert(entity);
        }

        /// <summary>
        /// ثبت رزمه جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public Guid CreateNewResume(ResumeCreateCommand command ,  string ipAddress)
        {
            var newResume = new Resume
            {
                CareerOpportunityId = command.CareerOpportunityId,
                AttachmentFileId = command.AttachmentFileId,
                IpAddress = ipAddress
            };
            Insert(newResume);
            Save();
            return newResume.Id;
        }

        /// <summary>
        /// حذف رزومه
        /// </summary>
        /// <param name="resumeId"></param>
        public void Delete(Guid resumeId)
        {
            _dataContext.ResumeRepository.Delete(resumeId);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
