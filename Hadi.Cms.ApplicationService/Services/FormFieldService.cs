using Hadi.Cms.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Model.Mappings.Mappers;

namespace Hadi.Cms.ApplicationService.Services
{
    public class FormFieldService
    {
        private readonly DataContext _dataContext;

        public FormFieldService()
        {
            _dataContext = new DataContext();
        }

        ~FormFieldService()
        {
            _dataContext?.Dispose();
        }
        
        /// <summary>
        /// دریافت لیست فیلدهای فرم
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<IFormFieldDto> GetList(Expression<Func<FormField, bool>> filter = null,
            Func<IQueryable<FormField>, IOrderedQueryable<FormField>> orderBy = null,
            params Expression<Func<FormField, object>>[] includes)
        {
            return _dataContext.FormFieldRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// دریافت یک فیلد بر اساس شناسه
        /// </summary>
        /// <param name="formFieldId"></param>
        /// <returns></returns>
        public IFormFieldDto Get(Guid formFieldId)
        {
            return _dataContext.FormFieldRepository.GetByID(formFieldId).MapToDto();
        }

        /// <summary>
        /// دریافت یک فیلد بر اساس شرط
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IFormFieldDto Get(Expression<Func<FormField, bool>> filter = null)
        {
            return _dataContext.FormFieldRepository.Get(filter).MapToDto();
        }

        /// <summary>
        /// ثبت فیلد فرم
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(FormField entity)
        {
            _dataContext.FormFieldRepository.Insert(entity);
        }

        public List<Guid> InsertRange(List<FormFieldCommand> formFieldsCommand , Guid formId ,Guid userId)
        {
            var newFormFieldIds = new List<Guid>();

            foreach (var item in formFieldsCommand)
            {
                var newFormField = new FormField()
                {
                    Label = item.Label,
                    Name = item.Name,
                    Type = item.Type,
                    CreatedBy = userId,
                    FormId = formId,
                    IsActive = true
                };
                Insert(newFormField);
                newFormFieldIds.Add(newFormField.Id);
            }

            Save();
            return newFormFieldIds;
        }

        public void Delete(Guid formFieldId)
        {
            _dataContext.FormFieldRepository.Delete(formFieldId);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
