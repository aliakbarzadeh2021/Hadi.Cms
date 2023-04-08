using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Repository.UnitOfWork;

namespace Hadi.Cms.ApplicationService.Services
{
    /// <summary>
    /// سرویس تگ های فایل پیوست شده
    /// </summary>
    public class AttachmentFileTagService
    {
        private readonly DataContext _dataContext;
        public AttachmentFileTagService()
        {
            _dataContext = new DataContext();
        }

        ~AttachmentFileTagService()
        {
            _dataContext.Dispose();
        }

        /// <summary>
        /// دریافت لیست تگ های فایل پیوست
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<AttachmentFileTag> GetList(Expression<Func<AttachmentFileTag, bool>> filter = null,
            Func<IQueryable<AttachmentFileTag>, IOrderedQueryable<AttachmentFileTag>> orderBy = null,
            params Expression<Func<AttachmentFileTag, object>>[] includes)
        {
            return _dataContext.AttachmentFileTagRepository.GetList(filter, orderBy, includes);
        }

        /// <summary>
        /// نسبت دادن تگ به فایل پیوست
        /// </summary>
        /// <param name="attachmentFileId"></param>
        /// <param name="tagsId"></param>
        /// <param name="userId"></param>
        public void AssignTagsToAttachmentFile(Guid attachmentFileId, List<Guid> tagsId, Guid userId)
        {
            // Delete old tags
            var oldTags = GetList(at => at.AttachmentFileId == attachmentFileId);
            foreach (var tag in oldTags)
            {
                DeleteById(tag.Id);
            }

            var attachment = _dataContext.AttachmentFileRepository.Get(at => at.Id == attachmentFileId);
            //Add new tag(s) for attachment file
            foreach (var newTagId in tagsId)
            {
                var tag = _dataContext.TagRepository.Get(t => t.Id == newTagId);
                var newAttachmentFileTag = new AttachmentFileTag()
                {
                    AttachmentFileId = attachmentFileId,
                    TagId = newTagId,
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    ModifiedBy = userId,
                    ModifiedDate = DateTime.Now,
                    Tag = tag,
                    AttachmentFile = attachment
                };
                Insert(newAttachmentFileTag);
            }
            Save();

        }

        public void Insert(AttachmentFileTag entity)
        {
            _dataContext.AttachmentFileTagRepository.Insert(entity);
        }

        public void DeleteById(Guid attachmentFileId)
        {
            _dataContext.AttachmentFileTagRepository.Delete(attachmentFileId);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
