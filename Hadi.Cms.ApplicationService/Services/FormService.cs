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
    /// سرویس فرم
    /// </summary>
    public class FormService
    {
        private readonly DataContext _dataContext;

        public FormService()
        {
            _dataContext = new DataContext();
        }

        ~FormService()
        {
            _dataContext?.Dispose();
        }


        /// <summary>
        /// دریافت لیست فرم ها
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<IFormDto> GetList(Expression<Func<Form, bool>> filter = null,
            Func<IQueryable<Form>, IOrderedQueryable<Form>> orderBy = null,
            params Expression<Func<Form, object>>[] includes)
        {
            return _dataContext.FormRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// دریافت اطلاعات یک فرم بر اساس شرط
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IFormDto Get(Expression<Func<Form,bool>> filter = null)
        {
            return _dataContext.FormRepository.Get(filter).MapToDto();
        }

        /// <summary>
        /// دریافت اطلاعات یک فرم بر اساس شناسه فرم
        /// </summary>
        /// <param name="formId"></param>
        /// <returns></returns>
        public IFormDto Get(Guid formId)
        {
            return _dataContext.FormRepository.GetByID(formId).MapToDto();
        }

        /// <summary>
        /// ثبت فرم جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Guid CreateNewForm(FormCreateCommand command, Guid userId)
        {
            var newForm = new Form()
            {
                Name = command.Name,
                Title = command.Title,
                LanguageId = command.LanguageId,
                DisplayEnName = command.DisplayEnName,
                DisplayFaName = command.DisplayFaName,
                RedirectUrl = command.RedirectUrl,
                FormDataSource = command.FormDataSource,
                CreatedBy = userId,
                IsActive = true
            };
            newForm.FormDataSource += $"<input type='hidden' data-form-builder='identifier' value='{newForm.Id}'/>";
            Insert(newForm);
            Save();
            return newForm.Id;
        }

        /// <summary>
        /// ثبت فرم جدید
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(Form entity)
        {
            _dataContext.FormRepository.Insert(entity);
        }

        /// <summary>
        /// ویرایش فرم
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        public void UpdateForm(Form entity, FormEditCommand command, Guid userId)
        {
            entity.Name = command.Name;
            entity.Title = command.Title;
            entity.RedirectUrl = command.RedirectUrl;
            entity.DisplayEnName = command.DisplayEnName;
            entity.DisplayFaName = command.DisplayFaName;
            entity.LanguageId = command.LanguageId;
            entity.FormDataSource = command.FormDataSource;
            entity.ModifiedBy = userId;
            entity.ModifiedDate =DateTime.Now;
            Update(entity);
            Save();
        }

        /// <summary>
        /// ویرایش فرم
        /// </summary>
        /// <param name="entity"></param>
        public void Update(Form entity)
        {
            _dataContext.FormRepository.Update(entity);
        }

        /// <summary>
        /// حذف فرم بر اساس شناسه
        /// </summary>
        /// <param name="formId"></param>
        public void Delete(Guid formId)
        {
            _dataContext.FormRepository.Delete(formId);
        }

        public bool Any(Expression<Func<Form, bool>> filter = null)
        {
            return _dataContext.FormRepository.Any(filter);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
