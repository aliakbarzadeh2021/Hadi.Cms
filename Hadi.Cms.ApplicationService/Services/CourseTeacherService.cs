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
    /// دوره های مدرس
    /// </summary>
    public class  CourseTeacherService
    {
        private readonly DataContext _dataContext;

        public CourseTeacherService()
        {
            _dataContext= new DataContext();
        }

        ~CourseTeacherService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست دوره های مدرس
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<ICourseTeacherDto> GetList(Expression<Func<CourseTeacher, bool>> filter = null,
            Func<IQueryable<CourseTeacher>, IOrderedQueryable<CourseTeacher>> orderBy = null,
            params Expression<Func<CourseTeacher, object>>[] includes)
        {
            return _dataContext.CourseTeacherRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// نسبت دادن مدرس به دوره
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="teachersId"></param>
        /// <param name="userId"></param>
        public void AssignTeachersToCourse(Guid courseId, List<Guid> teachersId, Guid userId)
        {
            #region Remove old teachers

            var courseTeachers = GetList(c => c.CourseId == courseId);
            foreach (var courseTeacher in courseTeachers)
            {
                Delete(courseTeacher.Id);
            }
            Save();

            #endregion

            if (teachersId != null && teachersId.Count > 0)
            {
                foreach (var teacherId in teachersId)
                {
                    var newCourseTeacher = new CourseTeacher
                    {
                        CourseId = courseId,
                        TeacherId = teacherId,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedBy = userId
                    };
                    Insert(newCourseTeacher);
                }
                Save();
            }
        }

        /// <summary>
        /// ثبت مدرس برای دوره
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(CourseTeacher entity)
        {
            _dataContext.CourseTeacherRepository.Insert(entity);
        }

        /// <summary>
        /// حذف دوره از مدرس
        /// </summary>
        /// <param name="courseTeacherId"></param>
        public void Delete(Guid courseTeacherId)
        {
            _dataContext.CourseTeacherRepository.Delete(courseTeacherId);
        }

        public void Save()
        {
            _dataContext.Save();
        }

    }
}
