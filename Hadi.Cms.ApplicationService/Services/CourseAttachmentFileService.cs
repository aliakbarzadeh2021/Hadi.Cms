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
    public class CourseAttachmentFileService
    {
        private readonly DataContext _dataContext;

        public CourseAttachmentFileService()
        {
            _dataContext = new DataContext();
        }

        ~CourseAttachmentFileService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست ویدیو های دوره
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<ICourseAttachmentFileDto> GetList(Expression<Func<CourseAttachmentFile, bool>> filter = null,
            Func<IQueryable<CourseAttachmentFile>, IOrderedQueryable<CourseAttachmentFile>> orderBy = null,
            params Expression<Func<CourseAttachmentFile, object>>[] includes)
        {
            return _dataContext.CourseAttachmentFileRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// نسبت دادن ویدیو به دوره
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="videosId"></param>
        /// <param name="userId"></param>
        public void AssignVideosToCourse(Guid courseId, List<Guid> videosId, Guid userId)
        {
            if (videosId != null && videosId.Count > 0)
            {
                foreach (var videoId in videosId)
                {
                    var newCourseAttachmentFile = new CourseAttachmentFile
                    {
                        CourseId = courseId,
                        AttachmentFileId = videoId,
                        CreatedBy = userId,
                        IsActive = true,
                        IsDeleted = false
                    };
                    Insert(newCourseAttachmentFile);
                }
                Save();
            }
        }

        /// <summary>
        /// ثبت ویدیو برای دوره
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(CourseAttachmentFile entity)
        {
            _dataContext.CourseAttachmentFileRepository.Insert(entity);
        }

        public void Delete(Guid courseAttachmentFileId)
        {
            _dataContext.CourseAttachmentFileRepository.Delete(courseAttachmentFileId);
        }

        /// <summary>
        /// حذف محدوده از ویدیو ها از دوره
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRange(IEnumerable<CourseAttachmentFile> entities)
        {
            _dataContext.CourseAttachmentFileRepository.DeleteRange(entities);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
