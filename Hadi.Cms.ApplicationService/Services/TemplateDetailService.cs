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
    public class TemplateDetailService
    {
        private DataContext _dataContext;

        public TemplateDetailService()
        {
            _dataContext = new DataContext();
        }

        public ITemplateDetailDto Get(Expression<Func<TemplateDetail, bool>> filter)
        {
            var templateDetail = _dataContext.TemplateDetailRepository.Get(filter);
            return templateDetail.MapToDto();
        }

        public ITemplateDetailDto GetById(object Id)
        {
            var templateDetail = _dataContext.TemplateDetailRepository.GetByID(Id);
            return templateDetail.MapToDto();
        }

        public List<ITemplateDetailDto> GetList(Expression<Func<TemplateDetail, bool>> filter = null)
        {
            var templateDetails = _dataContext.TemplateDetailRepository.GetList(filter);
            return templateDetails.MapToListDto();
        }

        public void Insert(TemplateDetail model)
        {
            _dataContext.TemplateDetailRepository.Insert(model);
        }

        public void Update(TemplateDetail model)
        {
            _dataContext.TemplateDetailRepository.Update(model);
        }

        public void Delete(TemplateDetail model)
        {
            _dataContext.TemplateDetailRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.TemplateDetailRepository.Delete(Id);
        }

        public void DeleteRange(IEnumerable<TemplateDetail> entities)
        {
            _dataContext.TemplateDetailRepository.DeleteRange(entities);
        }

        public IQueryable<ITemplateDetailDto> Where(Expression<Func<TemplateDetail, bool>> filter)
        {
            var templateDetails = _dataContext.TemplateDetailRepository.Where(filter);
            var templateDetailsDto = templateDetails.Select(q => q.MapToDto());
            return templateDetailsDto;
        }

        public bool Any(Expression<Func<TemplateDetail, bool>> filter)
        {
            return _dataContext.TemplateDetailRepository.Any(filter);
        }

        public int Count(Expression<Func<TemplateDetail, bool>> where = null)
        {
            return _dataContext.TemplateDetailRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
