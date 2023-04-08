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
    public class PageStatisticService
    {
        private DataContext _dataContext;

        public PageStatisticService()
        {
            _dataContext = new DataContext();
        }

        public IPageStatisticDto Get(Expression<Func<PageStatistic, bool>> filter)
        {
            var pageStatistic = _dataContext.PageStatisticRepository.Get(filter);
            return pageStatistic.MapToDto();
        }

        public IPageStatisticDto GetById(object Id)
        {
            var pageStatistic = _dataContext.PageStatisticRepository.GetByID(Id);
            return pageStatistic.MapToDto();
        }

        public List<IPageStatisticDto> GetList(Expression<Func<PageStatistic, bool>> filter = null)
        {
            var pageStatistics = _dataContext.PageStatisticRepository.GetList(filter);
            return pageStatistics.MapToListDto();
        }

        public void Insert(PageStatistic model)
        {
            _dataContext.PageStatisticRepository.Insert(model);
        }

        public void Update(PageStatistic model)
        {
            _dataContext.PageStatisticRepository.Update(model);
        }

        public void Delete(PageStatistic model)
        {
            _dataContext.PageStatisticRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.PageStatisticRepository.Delete(Id);
        }

        public void DeleteRange(IEnumerable<PageStatistic> entities)
        {
            _dataContext.PageStatisticRepository.DeleteRange(entities);
        }

        public IQueryable<IPageStatisticDto> Where(Expression<Func<PageStatistic, bool>> filter)
        {
            var pageStatistics = _dataContext.PageStatisticRepository.Where(filter);
            var pageStatisticsDto = pageStatistics.Select(q => q.MapToDto());
            return pageStatisticsDto;
        }

        public bool Any(Expression<Func<PageStatistic, bool>> filter)
        {
            return _dataContext.PageStatisticRepository.Any(filter);
        }

        public int Count(Expression<Func<PageStatistic, bool>> where = null)
        {
            return _dataContext.PageStatisticRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }

    }
}
