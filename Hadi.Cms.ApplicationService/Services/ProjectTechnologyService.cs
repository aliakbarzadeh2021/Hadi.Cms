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
    /// سرویس تکنولوژی های پروژه
    /// </summary>
    public class ProjectTechnologyService
    {
        private readonly DataContext _dataContext;

        public ProjectTechnologyService()
        {
            _dataContext = new DataContext();
        }

        ~ProjectTechnologyService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست تکنولوژی های پروژه
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<IProjectTechnologyDto> GetList(Expression<Func<ProjectTechnology, bool>> filter = null,
            Func<IQueryable<ProjectTechnology>, IOrderedQueryable<ProjectTechnology>> orderBy = null,
            params Expression<Func<ProjectTechnology, object>>[] includes)
        {
            return _dataContext.ProjectTechnologyRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// نسبت دادن تکنولوژی و ابزار به پروژه
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="technologiesId"></param>
        /// <param name="toolsId"></param>
        /// <param name="userId"></param>
        public void AssignTechnologiesAndToolsToProject(Guid projectId, List<Guid> technologiesId, List<Guid> toolsId, Guid userId)
        {

            var toolsAndTechnologiesId = new List<Guid>();

            if (technologiesId != null && technologiesId.Count > 0)
                toolsAndTechnologiesId.AddRange(technologiesId);

            if (toolsId != null && toolsId.Count > 0)
                toolsAndTechnologiesId.AddRange(toolsId);

            #region Remove old technologies and tools

            var oldTechnologies = GetList(pt => pt.ProjectId == projectId).MapToEntities();
            foreach (var projectTechnology in oldTechnologies)
            {
                Delete(projectTechnology.Id);
            }

            #endregion

            toolsAndTechnologiesId.RemoveAll(t => t == Guid.Empty);

            foreach (var toolAndTech in toolsAndTechnologiesId)
            {
                var newProjectTechnology = new ProjectTechnology
                {
                    ProjectId = projectId,
                    TechnologyId = toolAndTech,
                    CreatedBy = userId,
                    IsActive = true,
                    IsDeleted = false
                };
                Insert(newProjectTechnology);
            }
            Save();
        }

        /// <summary>
        /// ثبت تکنولوژی و ابزار برای پروژه
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(ProjectTechnology entity)
        {
            _dataContext.ProjectTechnologyRepository.Insert(entity);
        }

        /// <summary>
        /// حذف محدوده ای از تکنولوژی های پروژه
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRange(List<ProjectTechnology> entities)
        {
            _dataContext.ProjectTechnologyRepository.DeleteRange(entities);
        }

        /// <summary>
        /// حذف تکنولوژی از پروژه
        /// </summary>
        /// <param name="id"></param>
        public void Delete(Guid id)
        {
            _dataContext.ProjectTechnologyRepository.Delete(id);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
