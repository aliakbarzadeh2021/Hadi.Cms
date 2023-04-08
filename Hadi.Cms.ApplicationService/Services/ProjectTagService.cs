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
    /// <summary>
    /// سرویس تگ های پروژه
    /// </summary>
    public class ProjectTagService
    {
        // TODO : Implimentation project tag service .
        private readonly DataContext _dataContext;
        public ProjectTagService()
        {
            _dataContext = new DataContext();
        }

        ~ProjectTagService()
        {
            _dataContext.Dispose();
        }

        /// <summary>
        /// دریافت لیست تگ های پروژه ها
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<IProjectTagDto> GetList(Expression<Func<ProjectTag, bool>> filter = null,
            Func<IQueryable<ProjectTag>, IOrderedQueryable<ProjectTag>> orderBy = null,
            params Expression<Func<ProjectTag, object>>[] includes)
        {
            return _dataContext.ProjectTagRepository.GetList(filter , orderBy , includes).MapToListDto();
        }

        /// <summary>
        /// دریافت یک تگ پروژه بر اساس شرط
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IProjectTagDto Get(Expression<Func<ProjectTag, bool>> filter,
            params Expression<Func<ProjectTag, object>>[] includes)
        {
            return _dataContext.ProjectTagRepository.Get(filter, includes).MapToDto();
        }

        /// <summary>
        /// دریافت یک تگ پروژه بر اساس شناسه
        /// </summary>
        /// <param name="projectTagId"></param>
        /// <returns></returns>
        public IProjectTagDto Get(Guid projectTagId)
        {
            return _dataContext.ProjectTagRepository.GetByID(projectTagId).MapToDto();
        }

        /// <summary>
        /// ثبت تگ جدید برای پروژه
        /// </summary>
        /// <param name="model"></param>
        public void Insert(ProjectTag model)
        {
            _dataContext.ProjectTagRepository.Insert(model);
        }

        /// <summary>
        /// حذف یک تگ از پروژه
        /// </summary>
        /// <param name="model"></param>
        public void Delete(ProjectTag model)
        {
            _dataContext.ProjectTagRepository.Delete(model);
        }

        /// <summary>
        /// حذف یک تگ از پروژه بر اساس شناسه
        /// </summary>
        /// <param name="projectTagId"></param>
        public void Delete(Guid projectTagId)
        {
            _dataContext.ProjectTagRepository.Delete(projectTagId);
        }

        /// <summary>
        /// حذف یک بازه از تگ های پروژه
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRange(List<ProjectTag> entities)
        {
            _dataContext.ProjectTagRepository.DeleteRange(entities);
        }

        /// <summary>
        /// نسبت دادن تگ به پروژه
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="tagsId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Guid> AssignTagsToProject(Guid projectId, List<Guid> tagsId, Guid userId)
        {
            #region Remove old tags
            var oldTags = GetList(pt => pt.ProjectId == projectId);
            foreach (var tag in oldTags)
            {
                Delete(tag.Id);
            }
            #endregion

            var newProjectTagsIdList = new List<Guid>();
            foreach (var tagId in tagsId)
            {
                var newProjectTag = new ProjectTag
                {
                    ProjectId = projectId,
                    TagId = tagId,
                    CreatedBy = userId,
                    IsActive = true,
                    IsDeleted = false
                };
                Insert(newProjectTag);
                newProjectTagsIdList.Add(newProjectTag.Id);
            }
            Save();
            return newProjectTagsIdList;
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
