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
    public class TemplateService
    {
        private DataContext _dataContext;

        public TemplateService()
        {
            _dataContext = new DataContext();
        }

        public ITemplateDto Get(Expression<Func<Template, bool>> filter)
        {
            var template = _dataContext.TemplateRepository.Get(filter);
            return template.MapToDto();
        }

        public ITemplateDto GetById(object Id)
        {
            var template = _dataContext.TemplateRepository.GetByID(Id);
            return template.MapToDto();
        }

        public List<ITemplateDto> GetList(Expression<Func<Template, bool>> filter = null)
        {
            var templates = _dataContext.TemplateRepository.GetList(filter);
            return templates.MapToListDto();
        }

        public void Insert(Template model)
        {
            _dataContext.TemplateRepository.Insert(model);
        }

        public void Update(Template model)
        {
            _dataContext.TemplateRepository.Update(model);
        }

        public void Delete(Template model)
        {
            _dataContext.TemplateRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.TemplateRepository.Delete(Id);
        }

        public void DeleteRange(IEnumerable<Template> entities)
        {
            _dataContext.TemplateRepository.DeleteRange(entities);
        }

        public IQueryable<ITemplateDto> Where(Expression<Func<Template, bool>> filter)
        {
            var templates = _dataContext.TemplateRepository.Where(filter);
            var templatesDto = templates.Select(q => q.MapToDto());
            return templatesDto;
        }

        public bool Any(Expression<Func<Template, bool>> filter)
        {
            return _dataContext.TemplateRepository.Any(filter);
        }

        public int Count(Expression<Func<Template, bool>> where = null)
        {
            return _dataContext.TemplateRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
