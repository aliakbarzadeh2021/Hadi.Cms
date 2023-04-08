using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Permissions;
using Microsoft.Ajax.Utilities;
using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.Infrastructure.Helpers;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Repository.UnitOfWork;

namespace Hadi.Cms.ApplicationService.Services
{
    /// <summary>
    /// سرویس پروژه ها
    /// </summary>
    public class ProjectService
    {
        private readonly DataContext _dataContext;

        public ProjectService()
        {
            _dataContext = new DataContext();
        }

        ~ProjectService()
        {
            _dataContext.Dispose();
        }

        /// <summary>
        /// دریافت لیست پروژه ها
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<IProjectDto> GetList(Expression<Func<Project, bool>> filter = null,
            Func<IQueryable<Project>, IOrderedQueryable<Project>> orderBy = null,
            params Expression<Func<Project, object>>[] includes)
        {
            return _dataContext.ProjectRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// دریافت یک پروژه بر اساس شرط
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IProjectDto Get(Expression<Func<Project, bool>> filter,
            params Expression<Func<Project, object>>[] includes)
        {
            return _dataContext.ProjectRepository.Get(filter, includes).MapToDto();
        }

        /// <summary>
        /// دریافت یک پروژه بر اساس شناسسه
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IProjectDto Get(Guid projectId)
        {
            return _dataContext.ProjectRepository.GetByID(projectId).MapToDto();
        }

        /// <summary>
        /// جستجوی پروژه ها
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="strSearch"></param>
        /// <returns></returns>
        public List<IProjectDto> Search(Guid? serviceId, string strSearch)
        {
            var dto = new List<IProjectDto>();

            var projects = GetList(p => p.IsActive && !p.IsDeleted, null, p => p.ProjectTags,
                p => p.ProjectTags.Select(pt => pt.Tag));

            if (serviceId.HasValue)
            {
                var service = _dataContext.ServiceRepository.GetList(s => s.Id == serviceId, null, s
                        => s.ServiceTags,
                    s => s.ServiceTags.Select(st => st.Tag)).FirstOrDefault();

                if (service != null)
                {
                    var selectedTags = service.ServiceTags.Select(s => s.Tag).ToList();
                    foreach (var tag in selectedTags)
                        dto.AddRange(projects.Where(p =>
                                        p.ProjectTags.Any(pt => pt.TagId == tag.Id) && p.Title.Contains(strSearch)).ToList());

                }

                // remove duplicates from list
                dto = dto.DistinctBy(d => d.Id).ToList();

            }

            else
                dto.AddRange(projects.Where(p => p.Title.Contains(strSearch)));

            return dto.OrderByDescending(d => d.CreatedDate).ToList();
        }

        /// <summary>
        /// ثبت پروژه ی جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Guid CreateNewProject(ProjectCreateCommand command, Guid userId)
        {
            var newProject = new Project
            {
                EmployerId = command.EmployerId,
                Title = command.Title,
                ProjectType = command.ProjectType,
                PreviewImageGuid = command.PreviewImageGuid,
                ImageGuid = command.ImageGuid,
                ProjectLink = command.ProjectLink,
                ProjectLinkText = command.ProjectLinkText,
                Description = command.Description,
                CreatedBy = userId,
                IsActive = command.IsActive,
                FirstStepDescription = command.FirstStepDescription,
                FirstStepImage = command.FirstStepImage,
                SecondStepDescription = command.SecondStepDescription,
                FinalStepDescription = command.FinalStepDescription,
                IsDeleted = false
            };
            Insert(newProject);
            Save();
            return newProject.Id;
        }

        /// <summary>
        /// ویرایش پروژه
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public void UpdateProject(Project entity, ProjectEditCommand command, Guid userId)
        {
            entity.EmployerId = command.EmployerId;
            entity.Title = command.Title;
            entity.ProjectType = command.ProjectType;
            entity.PreviewImageGuid = command.PreviewImageGuid;
            entity.ImageGuid = command.ImageGuid;
            entity.ProjectLink = command.ProjectLink;
            entity.ProjectLinkText = command.ProjectLinkText;
            entity.ProjectLinkText = command.ProjectLinkText;
            entity.Description = command.Description;
            entity.FirstStepDescription = command.FirstStepDescription;
            entity.FirstStepImage = command.FirstStepImage;
            entity.SecondStepDescription = command.SecondStepDescription;
            entity.FinalStepDescription = command.FinalStepDescription;
            entity.ModifiedBy = userId;
            entity.ModifiedDate = DateTime.Now;
            entity.IsActive = command.IsActive;
            Update(entity);
            Save();
        }

        /// <summary>
        /// دریافت لیست پروژه های مشابه
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public List<IProjectDto> GetRelatedProjects(Guid projectId, int take = 4)
        {
            var projectsList = new List<IProjectDto>();
            var project = GetList(t => t.Id == projectId, null, p => p.ProjectTags, p => p.ProjectTags.Select(pt => pt.Tag))
                .FirstOrDefault();
            if (project != null)
            {
                var tags = project.ProjectTags.Select(pt => pt.TagDto).Where(t => t.IsActive).ToList();
                foreach (var tag in tags)
                {
                    var parentTag = _dataContext.TagRepository.Get(t => t.IsActive && !t.IsDeleted && t.Id == tag.ParentId);
                    if (parentTag != null)
                    {
                        var childrenTag = _dataContext.TagRepository.GetList(t => t.IsActive && !t.IsDeleted && t.ParentId == parentTag.Id);
                        foreach (var child in childrenTag)
                        {
                            var relatedProjects = GetList(p => p.ProjectTags.Any(pt => pt.TagId == child.Id) && p.IsActive && !p.IsDeleted && p.Id != projectId, null, p => p.ProjectTags, p => p.ProjectTags.Select(pt => pt.Tag));
                            projectsList.AddRange(relatedProjects);
                        }
                    }
                }

                projectsList = projectsList.DistinctBy(p => p.Id).OrderByDescending(p => p.CreatedDate).Take(take).ToList();
            }
            return projectsList;
        }

        /// <summary>
        /// ثبت پروژه جدید
        /// </summary>
        /// <param name="model"></param>
        public void Insert(Project model)
        {
            _dataContext.ProjectRepository.Insert(model);
        }

        /// <summary>
        /// ویرایش پروژه
        /// </summary>
        /// <param name="model"></param>
        public void Update(Project model)
        {
            _dataContext.ProjectRepository.Update(model);
        }

        /// <summary>
        /// حذف پروژه
        /// </summary>
        /// <param name="model"></param>
        public void Delete(Project model)
        {
            _dataContext.ProjectRepository.Delete(model);
        }

        /// <summary>
        /// حذف یک بازه از پروژها
        /// </summary>
        /// <param name="models"></param>
        public void DeleteRange(List<Project> models)
        {
            _dataContext.ProjectRepository.DeleteRange(models);
        }

        /// <summary>
        /// حذف پروژه بر اساس شناسه پروژه
        /// </summary>
        /// <param name="projectId"></param>
        public void Delete(Guid projectId)
        {
            _dataContext.ProjectRepository.Delete(projectId);
        }

        public bool Any(Expression<Func<Project, bool>> filter)
        {
            return _dataContext.ProjectRepository.Any(filter);
        }

        public int Count(Expression<Func<Project, bool>> filter = null)
        {
            return _dataContext.ProjectRepository.Count(filter);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
