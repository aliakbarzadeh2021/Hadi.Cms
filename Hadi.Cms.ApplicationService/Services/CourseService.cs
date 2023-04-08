using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Repository.UnitOfWork;

namespace Hadi.Cms.ApplicationService.Services
{
    /// <summary>
    /// سرویس دوره
    /// </summary>
    public class CourseService
    {
        private readonly DataContext _dataContext;

        public CourseService()
        {
            _dataContext = new DataContext();
        }

        ~CourseService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست دوره ها
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<ICourseDto> GetList(Expression<Func<Course, bool>> filter = null,
            Func<IQueryable<Course>, IOrderedQueryable<Course>> orderBy = null,
            params Expression<Func<Course, object>>[] includes)
        {
            var courses = _dataContext.CourseRepository.GetList(filter, orderBy, includes);

            return courses.MapToListDto();
        }

        /// <summary>
        /// دریافت یک دوره بر اساس شرط
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public ICourseDto Get(Expression<Func<Course, bool>> filter = null,
            params Expression<Func<Course, object>>[] includes)
        {
            return _dataContext.CourseRepository.Get(filter, includes).MapToDto();
        }

        /// <summary>
        /// دریافت یک دوره براساس شناسه دوره
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public ICourseDto Get(Guid courseId)
        {
            return _dataContext.CourseRepository.GetByID(courseId).MapToDto();
        }

        /// <summary>
        /// ثبت دوره جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Guid CreateNewCourse(CourseCreateCommand command, Guid userId)
        {
            var newCourse = new Course
            {
                Title = command.Title,
                PeriodOfTime = command.PeriodOfTime,
                Price = command.Price,
                AttachmentCoursePreviewVideoId = command.AttachmentCoursePreviewVideoId,
                CoursePreviewVideoPosterImageId = command.CoursePreviewVideoPosterImageId,
                AttachmentImageId = command.AttachmentImageId,
                Description = command.Description,
                IsActive = command.IsActive,
                CreatedBy = userId,
                IsDeleted = false,
            };
            Insert(newCourse);
            Save();
            return newCourse.Id;
        }

        /// <summary>
        /// ثبت دوره جدید
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(Course entity)
        {
            _dataContext.CourseRepository.Insert(entity);
        }

        /// <summary>
        /// ویرایش دوره
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        public void UpdateCourse(Course entity, CourseEditCommand command, Guid userId)
        {
            entity.Title = command.Title;
            entity.PeriodOfTime = command.PeriodOfTime;
            entity.Price = command.Price;
            entity.AttachmentImageId = command.AttachmentImageId;
            entity.CoursePreviewVideoPosterImageId = command.CoursePreviewVideoPosterImageId;
            entity.AttachmentCoursePreviewVideoId = command.AttachmentCoursePreviewVideoId;
            entity.Description = command.Description;
            entity.IsActive = command.IsActive;
            entity.ModifiedBy = userId;
            entity.ModifiedDate = DateTime.Now;
            Update(entity);
            Save();
        }

        /// <summary>
        /// ویرایش یک دوره
        /// </summary>
        /// <param name="entity"></param>
        public void Update(Course entity)
        {
            _dataContext.CourseRepository.Update(entity);
        }

        /// <summary>
        /// حذف یک دوره
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(Course entity)
        {
            _dataContext.CourseRepository.Delete(entity);
        }

        /// <summary>
        /// حذف یک دوره بر اساس شناسه دوره
        /// </summary>
        /// <param name="courseId"></param>
        public void Delete(Guid courseId)
        {
            _dataContext.CourseRepository.Delete(courseId);
        }

        /// <summary>
        /// حذف یک محدوده از دوره ها
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRange(IEnumerable<Course> entities)
        {
            _dataContext.CourseRepository.DeleteRange(entities);
        }

        public bool Any(Expression<Func<Course, bool>> filter = null)
        {
            return _dataContext.CourseRepository.Any(filter);
        }


        public int Count(Expression<Func<Course, bool>> filter = null)
        {
            return _dataContext.CourseRepository.Count(filter);
        }


        public void Save()
        {
            _dataContext.Save();
        }
    }
}
