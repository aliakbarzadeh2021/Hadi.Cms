using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hadi.Cms.ApplicationService.CommandModels;

namespace Hadi.Cms.ApplicationService.Services
{
    /// <summary>
    /// سرویس چالش
    /// </summary>
    public class ChallengeService
    {
        private readonly DataContext _dataContext;

        public ChallengeService()
        {
            _dataContext = new DataContext();
        }

        ~ChallengeService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست چالش ها
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<IChallengeDto> GetList(Expression<Func<Challenge, bool>> filter = null,
            Func<IQueryable<Challenge>, IOrderedQueryable<Challenge>> orderBy = null,
            params Expression<Func<Challenge, object>>[] includes)
        {
            return _dataContext.ChallengeRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// دریافت یک چالش بر اساس عبارت
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IChallengeDto Get(Expression<Func<Challenge, bool>> filter = null,
            params Expression<Func<Challenge, object>>[] includes)
        {
            return _dataContext.ChallengeRepository.Get(filter, includes).MapToDto();
        }

        /// <summary>
        /// دریافت یک چالش بر اساس شناسه
        /// </summary>
        /// <param name="challengeId"></param>
        /// <returns></returns>
        public IChallengeDto Get(Guid challengeId)
        {
            return _dataContext.ChallengeRepository.GetByID(challengeId).MapToDto();
        }

        /// <summary>
        /// ثبت یک چالش
        /// </summary>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Guid CreateNewChallenge(ChallengeCreateCommand command, Guid userId)
        {
            var newChallenge = new Challenge
            {
                Title = command.Title,
                Description = command.Description,
                ProblemDescription = command.ProblemDescription,
                ProblemSolvingDescription = command.ProblemSolvingDescription,
                ImageAttachmentId = command.ImageAttachmentId,
                VideoAttachmentId = command.VideoAttachmentId,
                CreatedBy = userId,
                IsActive = command.IsActive,
                IsDeleted = false
            };
            Insert(newChallenge);
            Save();
            return newChallenge.Id;
        }

        /// <summary>
        /// ثبت یک چالش
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(Challenge entity)
        {
            _dataContext.ChallengeRepository.Insert(entity);
        }

        /// <summary>
        /// ویرایش یک چالش
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateChallenge(Challenge entity, ChallengeEditCommand command, Guid userId)
        {
            try
            {
                entity.Title = command.Title;
                entity.Description = command.Description;
                entity.ProblemDescription = command.ProblemDescription;
                entity.ProblemSolvingDescription = command.ProblemSolvingDescription;
                entity.ImageAttachmentId = command.ImageAttachmentId;
                entity.VideoAttachmentId = command.VideoAttachmentId;
                entity.ModifiedBy = userId;
                entity.ModifiedDate = DateTime.Now;
                Update(entity);
                Save();
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// ویرایش یک چالش
        /// </summary>
        /// <param name="entity"></param>
        public void Update(Challenge entity)
        {
            _dataContext.ChallengeRepository.Update(entity);
        }

        /// <summary>
        /// حذف یک چالش
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(Challenge entity)
        {
            _dataContext.ChallengeRepository.Delete(entity);
        }

        /// <summary>
        /// حذف یک چالش
        /// </summary>
        /// <param name="challengeId"></param>
        public void Delete(Guid challengeId)
        {
            _dataContext.ChallengeRepository.Delete(challengeId);
        }

        /// <summary>
        /// حذف یک چالش
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRange(List<Challenge> entities)
        {
            _dataContext.ChallengeRepository.DeleteRange(entities);
        }

        public bool Any(Expression<Func<Challenge, bool>> filter = null)
        {
            return _dataContext.ChallengeRepository.Any(filter);
        }

        public int Count(Expression<Func<Challenge, bool>> filter = null)
        {
            return _dataContext.ChallengeRepository.Count(filter);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
