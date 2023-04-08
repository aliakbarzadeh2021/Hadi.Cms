using System.Collections.Generic;
using System.Linq.Expressions;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Repository.UnitOfWork;
using System;
using System.Linq;
using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Mappers;

namespace Hadi.Cms.ApplicationService.Services
{
    public class TeacherService
    {
        private readonly DataContext _dataContext;

        public TeacherService()
        {
            _dataContext = new DataContext();
        }

        ~TeacherService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست مدرسین
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<ITeacherDto> GetList(Expression<Func<Teacher, bool>> filter = null,
            Func<IQueryable<Teacher>, IOrderedQueryable<Teacher>> orderBy = null,
            params Expression<Func<Teacher, object>>[] includes)
        {
            return _dataContext.TeacherRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// دریافت یک مدرس بر اسا شرط
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public ITeacherDto Get(Expression<Func<Teacher, bool>> filter = null,
            params Expression<Func<Teacher, object>>[] includes)
        {
            return _dataContext.TeacherRepository.Get(filter, includes).MapToDto();
        }

        /// <summary>
        /// دریافت یک مدرس بر اساس شناسه
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public ITeacherDto Get(Guid teacherId)
        {
            return _dataContext.TeacherRepository.GetByID(teacherId).MapToDto();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Guid CreateNewTeacher(TeacherCreateCommand command, Guid userId)
        {
            var newTeacher = new Teacher
            {
                FullName = command.FullName,
                AttachmentImageId = command.AttachmentImageId,
                IsActive = command.IsActive,
                SocialNetworkName = command.SocialNetworkName,
                SocialNetworkLink = command.SocialNetworkLink,
                SocialNetworkImageGuid = command.SocialNetworkImageGuid,
                CreatedBy = userId
            };
            Insert(newTeacher);
            Save();
            return newTeacher.Id;
        }

        /// <summary>
        /// ثبت مدرس
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(Teacher entity)
        {
            _dataContext.TeacherRepository.Insert(entity);
        }

        /// <summary>
        /// ویرایش مدرس
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        public void UpdateTeacher(Teacher entity , TeacherEditCommand command , Guid userId)
        {
            entity.FullName = command.FullName;
            entity.AttachmentImageId = command.AttachmentImageId;
            entity.SocialNetworkName = command.SocialNetworkName;
            entity.SocialNetworkLink = command.SocialNetworkLink;
            entity.SocialNetworkImageGuid = command.SocialNetworkImageGuid;
            entity.IsActive = command.IsActive;
            entity.ModifiedBy = userId;
            entity.ModifiedDate  =DateTime.Now;
            Update(entity);
            Save();
        }

        public void Update(Teacher entity)
        {
            _dataContext.TeacherRepository.Update(entity);
        }

        /// <summary>
        /// حذف مدرس بر اساس شناسه
        /// </summary>
        /// <param name="teacherId"></param>
        public void Delete(Guid teacherId)
        {
            _dataContext.TeacherRepository.Delete(teacherId);
        }
        
        /// <summary>
        /// حذف مدرس
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(Teacher entity)
        {
            _dataContext.TeacherRepository.Delete(entity);
        }

        /// <summary>
        /// حذف محدوده ای از مدرسین
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRange(IEnumerable<Teacher> entities)
        {
            _dataContext.TeacherRepository.DeleteRange(entities);
        }

        public bool Any(Expression<Func<Teacher, bool>> filter = null)
        {
            return _dataContext.TeacherRepository.Any(filter);
        }

        public int Count(Expression<Func<Teacher, bool>> filter = null)
        {
            return _dataContext.TeacherRepository.Count(filter);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
