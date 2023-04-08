using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Quartz.Impl.AdoJobStore;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Repository.UnitOfWork;

namespace Hadi.Cms.ApplicationService.Services
{
    /// <summary>
    /// سرویس تگ های دوره
    /// </summary>
    public class CourseTagService
    {
        private readonly DataContext _dataContext;

        public CourseTagService()
        {
            _dataContext = new DataContext();
        }

        ~CourseTagService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست تگ های دوره
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<ICourseTagDto> GetList(Expression<Func<CourseTag, bool>> filter = null,
            Func<IQueryable<CourseTag>, IOrderedQueryable<CourseTag>> orderBy = null,
            params Expression<Func<CourseTag, object>>[] includes)
        {
            return _dataContext.CourseTagRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// نسبت دادن تگ به دوره
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="tagsId"></param>
        /// <param name="userId"></param>
        public void AssignTagsToCourse(Guid courseId, List<Guid> tagsId, Guid userId)
        {
            #region Remove old course tag

            var courseTags = GetList(c => c.CourseId == courseId);
            foreach (var courseTag in courseTags)
            {
                Delete(courseTag.Id);
            }
            Save();

            #endregion

            if (tagsId != null && tagsId.Count > 0)
            {
                foreach (var tagId in tagsId)
                {
                    var newCourseTag = new CourseTag
                    {
                        CourseId = courseId,
                        TagId = tagId,
                        CreatedBy = userId,
                        IsActive = true,
                        IsDeleted = false
                    };
                    Insert(newCourseTag);
                    Save();
                }
            }
        }

        /// <summary>
        /// ثبت تگ برای دوره
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(CourseTag entity)
        {
            _dataContext.CourseTagRepository.Insert(entity);
        }

        /// <summary>
        /// حذف تگ از دوره
        /// </summary>
        /// <param name="courseTagId"></param>
        public void Delete(Guid courseTagId)
        {
            _dataContext.CourseTagRepository.Delete(courseTagId);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
