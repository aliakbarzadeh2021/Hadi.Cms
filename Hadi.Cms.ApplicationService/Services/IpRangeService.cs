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
    public class IpRangeService
    {
        private DataContext _dataContext;

        public IpRangeService()
        {
            _dataContext = new DataContext();
        }

        public IIpRangeDto Get(Expression<Func<IpRange, bool>> filter)
        {
            var ipRange = _dataContext.IpRangeRepository.Get(filter);
            return ipRange.MapToDto();
        }

        public IIpRangeDto GetById(object Id)
        {
            var ipRange = _dataContext.IpRangeRepository.GetByID(Id);
            return ipRange.MapToDto();
        }

        public List<IIpRangeDto> GetList(Expression<Func<IpRange, bool>> filter = null , Func<IQueryable<IpRange>,IOrderedQueryable<IpRange>> orderBy = null , params Expression<Func<IpRange,object>>[] includes)
        {
            var ipRanges = _dataContext.IpRangeRepository.GetList(filter , orderBy , includes);
            return ipRanges.MapToListDto();
        }

        public void Insert(IpRange model)
        {
            _dataContext.IpRangeRepository.Insert(model);
        }

        public void Update(IpRange model)
        {
            _dataContext.IpRangeRepository.Update(model);
        }

        public void Delete(IpRange model)
        {
            _dataContext.IpRangeRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.IpRangeRepository.Delete(Id);
        }


        public void DeleteRange(IEnumerable<IpRange> entities)
        {
            _dataContext.IpRangeRepository.DeleteRange(entities);
        }

        public IQueryable<IIpRangeDto> Where(Expression<Func<IpRange, bool>> filter)
        {
            var ipRanges = _dataContext.IpRangeRepository.Where(filter);
            var ipRangesDto = ipRanges.Select(q => q.MapToDto());
            return ipRangesDto;
        }

        public bool Any(Expression<Func<IpRange, bool>> filter)
        {
            return _dataContext.IpRangeRepository.Any(filter);
        }

        public int Count(Expression<Func<IpRange, bool>> where = null)
        {
            return _dataContext.IpRangeRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }


        public static IpRangeDto MapToDto(IpRange model)
        {
            var ipRangeDto = new IpRangeDto
            {
                CreateDate = model.CreateDate,
                CreatedBy = model.CreatedBy,
                IsActive = model.IsActive,
                Id = model.Id,
                Lower = model.Lower,
                Upper = model.Upper
            };
            return ipRangeDto;
        }

    }
}
