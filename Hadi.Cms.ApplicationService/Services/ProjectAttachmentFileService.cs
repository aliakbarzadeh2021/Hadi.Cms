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
    /// سرویس فایل های پروژه
    /// </summary>
    public class ProjectAttachmentFileService
    {
        private readonly DataContext _dataContext;

        public ProjectAttachmentFileService()
        {
            _dataContext = new DataContext();
        }

        ~ProjectAttachmentFileService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست فایلاهای پروژه
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<IProjectAttachmentFileDto> GetList(Expression<Func<ProjectAttachmentFile, bool>> filter = null,
            Func<IQueryable<ProjectAttachmentFile>, IOrderedQueryable<ProjectAttachmentFile>> orderBy = null,
            params Expression<Func<ProjectAttachmentFile, object>>[] includes)
        {
            return _dataContext.ProjectAttachmentFileRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// دریافت فایل پروژه
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IProjectAttachmentFileDto Get(Expression<Func<ProjectAttachmentFile, bool>> filter = null,
            params Expression<Func<ProjectAttachmentFile, object>>[] includes)
        {
            return _dataContext.ProjectAttachmentFileRepository.Get(filter, includes).MapToDto();
        }

        /// <summary>
        /// نسبتدادن عکس اسلایدر به پروژه
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="sliderImagesId"></param>
        /// <param name="userId"></param>
        public void AssignSliderImageForProject(Guid projectId, List<Guid> sliderImagesId, Guid userId)
        {
            foreach (var sliderImageId in sliderImagesId)
            {
                var newProjectAttachmentFile = new ProjectAttachmentFile
                {
                    ProjectId = projectId,
                    AttachmentFileId = sliderImageId,
                    CreatedBy = userId,
                    IsActive = true,
                    IsDeleted = false,
                    
                };
                Insert(newProjectAttachmentFile);
            }
            Save();
        }

        /// <summary>
        /// ثبت فایل برا یروژه
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(ProjectAttachmentFile entity)
        {
            _dataContext.ProjectAttachmentFileRepository.Insert(entity);
        }

        /// <summary>
        /// حذف یک فایل از پروژه بر اساس شناسه
        /// </summary>
        /// <param name="projectAttachmentFileId"></param>
        public void Delete(Guid projectAttachmentFileId)
        {
            _dataContext.ProjectAttachmentFileRepository.Delete(projectAttachmentFileId);
        }

        /// <summary>
        /// حذف یک فایل از پروژه
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(ProjectAttachmentFile entity)
        {
            _dataContext.ProjectAttachmentFileRepository.Delete(entity);
        }

        /// <summary>
        /// حذف یک محدوده از فایل های پروژه
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRange(List<ProjectAttachmentFile> entities)
        {
            _dataContext.ProjectAttachmentFileRepository.DeleteRange(entities);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
