using Hadi.Cms.ApplicationService.QueryModels;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Hadi.Cms.ApplicationService.Services
{
    public class IpBannedService
    {
        private DataContext _dataContext;

        public IpBannedService()
        {
            _dataContext = new DataContext();
        }

        public IIpBannedDto Get(Expression<Func<IpBanned, bool>> filter)
        {
            var ipBanned = _dataContext.IpBannedRepository.Get(filter);
            return ipBanned.MapToDto();
        }

        public IIpBannedDto GetById(object Id)
        {
            var ipBanned = _dataContext.IpBannedRepository.GetByID(Id);
            return ipBanned.MapToDto();
        }

        public List<IIpBannedDto> GetList(Expression<Func<IpBanned, bool>> filter = null, Func<IQueryable<IpBanned>, IOrderedQueryable<IpBanned>> orderBy = null, params Expression<Func<IpBanned, object>>[] includes)
        {
            var ipBanneds = _dataContext.IpBannedRepository.GetList(filter, orderBy, includes);
            return ipBanneds.MapToListDto();
        }

        public void Insert(IpBanned model)
        {
            _dataContext.IpBannedRepository.Insert(model);
        }

        public void Update(IpBanned model)
        {
            _dataContext.IpBannedRepository.Update(model);
        }

        public void Delete(IpBanned model)
        {
            _dataContext.IpBannedRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.IpBannedRepository.Delete(Id);
        }


        public void DeleteRange(IEnumerable<IpBanned> entities)
        {
            _dataContext.IpBannedRepository.DeleteRange(entities);
        }

        public IQueryable<IIpBannedDto> Where(Expression<Func<IpBanned, bool>> filter)
        {
            var ipBanneds = _dataContext.IpBannedRepository.Where(filter);
            var ipBannedsDto = ipBanneds.Select(q => q.MapToDto());
            return ipBannedsDto;
        }

        public bool Any(Expression<Func<IpBanned, bool>> filter)
        {
            return _dataContext.IpBannedRepository.Any(filter);
        }

        public int Count(Expression<Func<IpBanned, bool>> where = null)
        {
            return _dataContext.IpBannedRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }


        public static IpBannedDto MapToDto(IpBanned model)
        {
            var ipBannedDto = new IpBannedDto
            {
                CreateDate = model.CreateDate,
                CreatedBy = model.CreatedBy,
                Id = model.Id,
                IpAddress = model.IpAddress,
                IpAddressBanReason = model.IpAddressBanReason,
                IsActive = model.IsActive
            };
            return ipBannedDto;
        }
    }
}
