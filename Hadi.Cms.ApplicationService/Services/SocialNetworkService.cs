using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hadi.Cms.Infrastructure.Helpers;

namespace Hadi.Cms.ApplicationService.Services
{
    /// <summary>
    /// سرویس شبکه های اجتماعی
    /// </summary>
    public class SocialNetworkService
    {
        private readonly DataContext _dataContext;
        public SocialNetworkService()
        {
            _dataContext = new DataContext();
        }

        ~SocialNetworkService()
        {
            _dataContext.Dispose();
        }

        /// <summary>
        /// دریافت لیست شبکه های اجتماعی
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<ISocialNetwork> GetList(Expression<Func<SocialNetwork, bool>> filter = null,
            Func<IQueryable<SocialNetwork>, IOrderedQueryable<SocialNetwork>> orderBy = null,
            params Expression<Func<SocialNetwork, object>>[] includes)
        {
            var socialNetworkList = _dataContext.SocialNetworkRepository.GetList(filter, orderBy, includes);
            return socialNetworkList.MapToDto();
        }

        /// <summary>
        /// دریافت یک شبکه های اجتماعی
        /// </summary>
        /// <param name="socialNetworkId"></param>
        /// <returns></returns>
        public ISocialNetwork GetByID(Guid socialNetworkId)
        {
            var socialNetwork = _dataContext.SocialNetworkRepository.GetByID(socialNetworkId);
            return socialNetwork.MapToDto();
        }

        /// <summary>
        /// حذف شبکه ی اجتماعی
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(SocialNetwork entity)
        {
            _dataContext.SocialNetworkRepository.Delete(entity);
            Save();
        }

        /// <summary>
        /// حذف شبکه ی اجتماعی
        /// </summary>
        /// <param name="socialNetworkId"></param>
        public void Delete(Guid socialNetworkId)
        {
            _dataContext.SocialNetworkRepository.Delete(socialNetworkId);
            Save();
        }

        /// <summary>
        /// ثبت شبکه اجتماعی جدید
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(SocialNetwork entity)
        {
            _dataContext.SocialNetworkRepository.Insert(entity);
            Save();
        }

        /// <summary>
        /// ویرایش شبکه اجتماعی
        /// </summary>
        /// <param name="command"></param>
        /// <param name="entity"></param>
        /// <param name="userId"></param>
        public void Update(SocialNetworkEditCommand command , ISocialNetwork entity , Guid userId)
        {
            entity.Title = command.Title;
            entity.Link = command.Link;
            entity.Source = command.Source?.PersianToEnglishNumber();
            entity.ModifiedBy = userId;
            entity.ModifiedDate = DateTime.Now;
            entity.ImageGuid = command.ImageGuid;
           
            entity.IsActive = command.IsActive;
            Update(entity.MapToEntity());
            Save();
        }

        public void Update(SocialNetwork entity)
        {
            _dataContext.SocialNetworkRepository.Update(entity);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
