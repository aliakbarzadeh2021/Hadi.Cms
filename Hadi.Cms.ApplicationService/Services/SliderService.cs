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
    /// سرویس اسلایدر
    /// </summary>
    public class SliderService
    {
        private readonly DataContext _dataContext;

        public SliderService()
        {
            _dataContext = new DataContext();
        }

        ~SliderService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست اسلایدرها
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<ISliderDto> GetList(Expression<Func<Slider, bool>> filter = null,
            Func<IQueryable<Slider>, IOrderedQueryable<Slider>> orderBy = null,
            params Expression<Func<Slider, object>>[] includes)
        {
            return _dataContext.SliderRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// دریافت یک اسلایدر بر اساس شرط
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public ISliderDto Get(Expression<Func<Slider, bool>> filter, params Expression<Func<Slider, object>>[] includes)
        {
            return _dataContext.SliderRepository.Get(filter, includes).MapToDto();
        }

        /// <summary>
        /// دریافت یک اسلایدر بر اساس شناسه اسلایدر
        /// </summary>
        /// <param name="sliderId"></param>
        /// <returns></returns>
        public ISliderDto Get(Guid sliderId)
        {
            return _dataContext.SliderRepository.GetByID(sliderId).MapToDto();
        }

        /// <summary>
        /// ثبت اسلایدر جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Guid CreateNewSlider(SliderCreateCommand command, Guid userId)
        {
            var newSlider = new Slider
            {
                Title = command.Title,
                Description = command.Description?.Replace("\r\n", "<br/>"),
                IsActive = command.IsActive,
                CreatedBy = userId
            };
            Insert(newSlider);
            Save();
            return newSlider.Id;
        }

        /// <summary>
        /// ثبت اسلایدر جدید
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(Slider entity)
        {
            _dataContext.SliderRepository.Insert(entity);
        }

        /// <summary>
        /// ویرایش اسلایدر
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        public void UpdateSlider(Slider entity, SliderEditCommand command, Guid userId)
        {
            entity.Title = command.Title;
            entity.Description = command.Description?.Replace("\r\n","<br/>");
            entity.ModifiedBy = userId;
            entity.ModifiedDate = DateTime.Now;
            Update(entity);
            Save();
        }

        /// <summary>
        /// ویرایش اسلایدر
        /// </summary>
        /// <param name="entity"></param>
        public void Update(Slider entity)
        {
            _dataContext.SliderRepository.Update(entity);
        }

        /// <summary>
        /// حذف اسلایدر بر اساس شناسه
        /// </summary>
        /// <param name="sliderId"></param>
        public void Delete(Guid sliderId)
        {
            _dataContext.SliderRepository.Delete(sliderId);
        }

        /// <summary>
        /// حذف اسلایدر
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(Slider entity)
        {
            _dataContext.SliderRepository.Delete(entity);
        }

        public bool Any(Expression<Func<Slider, bool>> filter = null)
        {
            return _dataContext.SliderRepository.Any(filter);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
