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
    /// سرویس مقدار فیلدهای فرم
    /// </summary>
    public class FormFieldValueService
    {
        private readonly DataContext _dataContext;

        public FormFieldValueService()
        {
            _dataContext = new DataContext();
        }

        public List<IFormFieldValueDto> GetList(Expression<Func<FormFieldValue, bool>> filter = null,
            Func<IQueryable<FormFieldValue>, IOrderedQueryable<FormFieldValue>> orderBy = null,
            params Expression<Func<FormFieldValue, object>>[] includes)
        {
            return _dataContext.FormFieldValueRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        public void Insert(FormFieldValue entity)
        {
            _dataContext.FormFieldValueRepository.Insert(entity);
        }

        public void DeleteRange(List<IFormFieldValueDto> entities)
        {
            foreach (var entity in entities)
                _dataContext.FormFieldValueRepository.Delete(entity.Id);
        }

        public void Save()
        {
            _dataContext.Save();
        }

        ~FormFieldValueService()
        {
            _dataContext?.Dispose();
        }
    }
}
