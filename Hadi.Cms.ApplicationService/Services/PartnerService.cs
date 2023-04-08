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
    /// سرویس همکاران
    /// </summary>
    public class PartnerService
    {
        private readonly DataContext _dataContext;

        public PartnerService()
        {
            _dataContext = new DataContext();
        }

        ~PartnerService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست همکاارن
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<IPartnerDto> GetList(Expression<Func<Partner, bool>> filter = null,
            Func<IQueryable<Partner>, IOrderedQueryable<Partner>> orderBy = null,
            params Expression<Func<Partner, object>>[] includes)
        {
            return _dataContext.PartnerRepository.GetList(filter, orderBy, includes).MapToListDto();
        }


        /// <summary>
        /// دریافت اطلاعات همکار بر اساس شناسه
        /// </summary>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public IPartnerDto Get(Guid partnerId)
        {
            return _dataContext.PartnerRepository.GetByID(partnerId).MapToDto();
        }

        /// <summary>
        /// ثبت همکار جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Guid AddNewPartner(PartnerCreateCommand command, Guid userId)
        {
            var newPartner = new Partner
            {
                Name = command.Name,
                AttachmentImageGuid = command.AttachmentImageGuid,
                Link = command.Link,
                ToolTip = command.ToolTip,
                IsActive = command.IsActive,
                CreatedBy = userId
            };
            Insert(newPartner);
            Save();
            return newPartner.Id;
        }

        /// <summary>
        /// ثبت همکار جدید
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(Partner entity)
        {
            _dataContext.PartnerRepository.Insert(entity);
        }

        public void UpdatePartner(Partner entity, PartnerEditCommand command, Guid userId)
        {
            entity.Name = command.Name;
            entity.AttachmentImageGuid = command.AttachmentImageGuid;
            entity.Link = command.Link;
            entity.ToolTip = command.ToolTip;
            entity.IsActive = command.IsActive;
            entity.ModifiedBy = userId;
            entity.ModifiedDate = DateTime.Now;
            Update(entity);
            Save();
        }

        /// <summary>
        /// ویرایش اطلاعات همکار
        /// </summary>
        /// <param name="entity"></param>
        public void Update(Partner entity)
        {
            _dataContext.PartnerRepository.Update(entity);
        }

        /// <summary>
        /// حذف همکار
        /// </summary>
        /// <param name="partnerId"></param>
        public void Delete(Guid partnerId)
        {
            _dataContext.PartnerRepository.Delete(partnerId);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
