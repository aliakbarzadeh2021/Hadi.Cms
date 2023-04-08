using AutoMapper;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using Hadi.Cms.Model.Mappings.Mappers;
using System.Linq.Expressions;

namespace Hadi.Cms.ApplicationService.Services
{
    /// <summary>
    /// سرویس تگ های چالش
    /// </summary>
    public class ChallengeTagService
    {
        private readonly DataContext _dataContext;

        public ChallengeTagService()
        {
            _dataContext = new DataContext();
        }

        ~ChallengeTagService()
        {
            _dataContext?.Dispose();
        }

        public List<IChallengeTagDto> GetList(Expression<Func<ChallengeTag, bool>> filter = null,
            Func<IQueryable<ChallengeTag>, IOrderedQueryable<ChallengeTag>> orderBy = null, params Expression<Func<ChallengeTag, object>>[] includes)
        {
            var challengeTags = _dataContext.ChallengeTagRepository.GetList(filter, orderBy, includes);
            return challengeTags.MapToListDto();
        }


        /// <summary>
        /// نسبت دادن برچسب به چالش
        /// </summary>
        /// <param name="tagsId"></param>
        /// <param name="challengeId"></param>
        /// <param name="userId"></param>
        public void AssignTagsToChallenge(List<Guid> tagsId , Guid challengeId, Guid userId)
        {
            var challengeTags = GetList(c => c.ChallengeId == challengeId);
            
            foreach (var item in challengeTags)
                Delete(item.Id);

            if(tagsId != null && tagsId.Count > 0)
            {
                foreach (var tagId in tagsId)
                {
                    var newChallengeTag = new ChallengeTag
                    {
                        ChallengeId = challengeId,
                        TagId = tagId,
                        CreatedBy = userId
                    };
                    Insert(newChallengeTag);
                }
            }
            Save();
        }

        /// <summary>
        /// ثبت تگ برای چالش
        /// </summary>
        /// <param name="model"></param>
        public void Insert(ChallengeTag model)
        {
            _dataContext.ChallengeTagRepository.Insert(model);
        }

        /// <summary>
        /// حذف تگ از چالش
        /// </summary>
        /// <param name="challengeTagId"></param>
        public void Delete(Guid challengeTagId)
        {
            _dataContext.ChallengeTagRepository.Delete(challengeTagId);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
