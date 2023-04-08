using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Repository.UnitOfWork;

namespace Hadi.Cms.ApplicationService.Services
{
    /// <summary>
    /// سرویس چالش های پروژه
    /// </summary>
    public class ChallengeProjectService
    {
        private readonly DataContext _dataContext;

        public ChallengeProjectService()
        {
            _dataContext = new DataContext();
        }

        ~ChallengeProjectService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست چالش های پروژه
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<IChallengeProjectDto> GetList(Expression<Func<ChallengeProject, bool>> filter = null,
            Func<IQueryable<ChallengeProject>, IOrderedQueryable<ChallengeProject>> orderBy = null,
            params Expression<Func<ChallengeProject, object>>[] includes)
        {
            return _dataContext.ChallengeProjectRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// دریافت یک چالش پروژه
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IChallengeProjectDto Get(Expression<Func<ChallengeProject, bool>> filter,
            params Expression<Func<ChallengeProject, object>>[] includes)
        {
            return _dataContext.ChallengeProjectRepository.Get(filter, includes).MapToDto();
        }

        /// <summary>
        /// دریافت یک چالش بر اساس شناسه
        /// </summary>
        /// <param name="challengeProjectId"></param>
        /// <returns></returns>
        public IChallengeProjectDto Get(Guid challengeProjectId)
        {
            return _dataContext.ChallengeProjectRepository.GetByID(challengeProjectId).MapToDto();
        }

        /// <summary>
        /// نسبت دادن پروژه به چالش
        /// </summary>
        /// <param name="challengeId"></param>
        /// <param name="projects"></param>
        /// <param name="userId"></param>
        public void AssignProjectsToChallenge(Guid challengeId, List<Guid> projects, Guid userId)
        {
            #region Remove old project
            var oldProjects = GetList(cp => cp.ChallengeId == challengeId).MapToEntities();
            foreach (var project in oldProjects)
            {
                Delete(project.Id);
            }
            #endregion

            foreach (var projectId in projects)
            {
                var newChallengeProject = new ChallengeProject
                {
                    ChallengeId = challengeId,
                    ProjectId = projectId,
                    CreatedBy = userId,
                    IsActive = true,
                    IsDeleted = false
                };
                Insert(newChallengeProject);
            }
            Save();
        }

        /// <summary>
        /// ثبت چالش برای پروژه
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(ChallengeProject entity)
        {
            _dataContext.ChallengeProjectRepository.Insert(entity);
        }

        /// <summary>
        /// ویرایش چالش پروژه
        /// </summary>
        /// <param name="entity"></param>
        public void Update(ChallengeProject entity)
        {
            _dataContext.ChallengeProjectRepository.Update(entity);
        }

        /// <summary>
        /// حذف چالش از پروژه
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(ChallengeProject entity)
        {
            _dataContext.ChallengeProjectRepository.Delete(entity);
        }

        /// <summary>
        /// حذف چالش از پروژه بر اساس شناسه
        /// </summary>
        /// <param name="challengeProjectId"></param>
        public void Delete(Guid challengeProjectId)
        {
            _dataContext.ChallengeProjectRepository.Delete(challengeProjectId);
        }

        /// <summary>
        /// حذف محدوده ای از چالش های پروژه
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRange(List<ChallengeProject> entities)
        {
            _dataContext.ChallengeProjectRepository.DeleteRange(entities);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
