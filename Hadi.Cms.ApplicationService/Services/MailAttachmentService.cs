using Hadi.Cms.ApplicationService.QueryModels;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Hadi.Cms.ApplicationService.Services
{
    public class MailAttachmentService
    {
        private DataContext _dataContext;

        public MailAttachmentService()
        {
            _dataContext = new DataContext();
        }

        public MailAttachmentDto Get(Expression<Func<MailAttachment, bool>> filter)
        {
            var mailAttachment = _dataContext.MailAttachmentRepository.Get(filter);
            var mailAttachmentDto = MapToDto(mailAttachment);
            return mailAttachmentDto;
        }

        public MailAttachmentDto GetById(object Id)
        {
            var mailAttachment = _dataContext.MailAttachmentRepository.GetByID(Id);
            var mailAttachmentDto = MapToDto(mailAttachment);
            return mailAttachmentDto;
        }

        public List<MailAttachmentDto> GetList(Expression<Func<MailAttachment, bool>> filter = null)
        {
            var mailAttachments = _dataContext.MailAttachmentRepository.GetList(filter);
            var mailAttachmentsDto = mailAttachments.Select(MapToDto);
            return mailAttachmentsDto.ToList();
        }

        public void Insert(MailAttachment model)
        {
            _dataContext.MailAttachmentRepository.Insert(model);
        }

        public void Update(MailAttachment model)
        {
            _dataContext.MailAttachmentRepository.Update(model);
        }

        public void Delete(MailAttachment model)
        {
            _dataContext.MailAttachmentRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.MailAttachmentRepository.Delete(Id);
        }

        public void DeleteRange(IEnumerable<MailAttachment> entities)
        {
            _dataContext.MailAttachmentRepository.DeleteRange(entities);
        }

        public IQueryable<MailAttachment> Where(Expression<Func<MailAttachment, bool>> filter)
        {
            return _dataContext.MailAttachmentRepository.Where(filter);
        }

        public bool Any(Expression<Func<MailAttachment, bool>> filter)
        {
            return _dataContext.MailAttachmentRepository.Any(filter);
        }

        public int Count(Expression<Func<MailAttachment, bool>> where = null)
        {
            return _dataContext.MailAttachmentRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }



        public static MailAttachmentDto MapToDto(MailAttachment model)
        {
            var mailAttachmentDto = new MailAttachmentDto
            {
                AttachmentId = model.AttachmentId,
                Id = model.Id,
                MailId = model.MailId
            };
            return mailAttachmentDto;
        }
    }
}
