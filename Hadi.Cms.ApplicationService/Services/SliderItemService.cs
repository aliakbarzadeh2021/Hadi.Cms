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
    ///سرویس ایتم های سالایدر
    /// </summary>
    public class SliderItemService
    {
        private readonly DataContext _dataContext;

        public SliderItemService()
        {
            _dataContext = new DataContext();
        }

        ~SliderItemService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست ایتم های اسلایدر
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<ISliderItemDto> GetList(Expression<Func<SliderItem, bool>> filter = null,
            Func<IQueryable<SliderItem>, IOrderedQueryable<SliderItem>> orderBy = null,
            params Expression<Func<SliderItem, object>>[] includes)
        {
            return _dataContext.SliderItemRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// دریافت یک ایتم از اسلایدر بر اساس شناسه
        /// </summary>
        /// <param name="sliderItemId"></param>
        /// <returns></returns>
        public ISliderItemDto Get(Guid sliderItemId)
        {
            return _dataContext.SliderItemRepository.GetByID(sliderItemId).MapToDto();
        }

        /// <summary>
        /// دریافت یک ایتم اسلایدر بر اساس شرط
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public ISliderItemDto Get(Expression<Func<SliderItem, bool>> filter = null,
            params Expression<Func<SliderItem, object>>[] includes)
        {
            return _dataContext.SliderItemRepository.Get(filter, includes).MapToDto();
        }

        /// <summary>
        /// ثبت ایتم اسلایدر جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Guid CreateNewSliderItem(SliderItemCreateCommand command, Guid userId)
        {
            var newSliderItem = new SliderItem
            {
                SliderId = command.SliderId,
                OrderId = command.OrderId,
                Title = command.Title?.Replace("\r\n", "<br/>"),
                SubTitle = command.SubTitle,
                Description = command.Description?.Replace("\r\n","<br/>"),
                ButtonText = command.ButtonText,
                ButtonLink = command.ButtonLink,
                ButtonCss = command.ButtonCss,
                AttachmentImageId = command.AttachmentImageId,
                IsActive = command.IsActive,
                CreatedBy = userId
            };
            Insert(newSliderItem);
            Save();
            return newSliderItem.Id;
        }

        /// <summary>
        /// ثبت ایتم برای اسلایدر
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(SliderItem entity)
        {
            _dataContext.SliderItemRepository.Insert(entity);
        }

        /// <summary>
        /// ویرایش ایتم اسلایدر
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        public void UpdateSliderItem(SliderItem entity, SliderItemEditCommand command, Guid userId)
        {
            entity.Title = command.Title?.Replace("\r\n", "<br/>");
            entity.SubTitle = command.SubTitle;
            entity.Description = command.Description?.Replace("\r\n", "<br/>");
            entity.OrderId = command.OrderId;
            entity.AttachmentImageId = command.AttachmentImageId;
            entity.ButtonText = command.ButtonText;
            entity.ButtonLink = command.ButtonLink;
            entity.ButtonCss = command.ButtonCss;
            entity.ModifiedBy = userId;
            entity.ModifiedDate = DateTime.Now;
            Update(entity);
            Save();
        }

        /// <summary>
        /// ویرایش ایتم اسلایدر
        /// </summary>
        /// <param name="entity"></param>
        public void Update(SliderItem entity)
        {
            _dataContext.SliderItemRepository.Update(entity);
        }

        /// <summary>
        /// حذف ایتم های اسلایدر
        /// </summary>
        /// <param name="sliderItemId"></param>
        public void Delete(Guid sliderItemId)
        {
            _dataContext.SliderItemRepository.Delete(sliderItemId);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
