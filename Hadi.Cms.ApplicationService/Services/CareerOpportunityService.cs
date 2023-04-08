using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Hadi.Cms.ApplicationService.CommandModels;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Repository.UnitOfWork;

namespace Hadi.Cms.ApplicationService.Services
{
    /// <summary>
    /// سرویس فرصت های شغلی
    /// </summary>
    public class CareerOpportunityService
    {
        private readonly DataContext _dataContext;

        public CareerOpportunityService()
        {
            _dataContext = new DataContext();
        }

        /// <summary>
        /// جنریت کردن محتوای صفحه
        /// </summary>
        /// <param name="source"></param>
        /// <param name="variable"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public string GenerateContentHtml(string source, string variable, ICareerOpportunityDto dto)
        {
            var html = new StringBuilder();

            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(variable))
                return html.ToString();


            variable = variable.ToLower();

            if (variable.Contains("careeropportunity"))
            {
                html.AppendLine($"<div id={dto.Id} class='card-container'>");
                html.AppendLine($"<div class='main-container'>");
                html.AppendLine("<div class='title'>");
                html.AppendLine($"{dto.Title}");
                html.AppendLine("</div>");
                html.AppendLine("<div class='description text-medium'>");
                html.AppendLine($"{dto.Description}");
                html.AppendLine("<div class='image-container'>");
                html.AppendLine($"<img src='{dto.CareerOpportunityImageSource}' alt=''>");
                html.AppendLine("</div>");
                html.AppendLine("</div>");
                html.AppendLine("<div class='hover-information'>");
                html.AppendLine("<div class='description'>");
                html.AppendLine("لطفا رزومه خود را آپلود فرمایید. همکاران بخش منابع انسانی هادی با شما تماس خواهند گرفت.");
                html.AppendLine("</div>");
                html.AppendLine("<div class='btn-container'>");
                html.AppendLine("<div class='btn-custom'>");
                html.AppendLine("<img src='/Content/Images/Hadi/Career/attach.svg' alt=''>");
                html.AppendLine("<span class='Dropzone' id='my-awesome'>");
                html.AppendLine("آپلود رزومه");
                html.AppendLine("</span>");
                html.AppendLine("</div>");
                html.AppendLine("</div>");
                html.AppendLine("</div>");
                html.AppendLine("</div>");
                html.AppendLine("</div>");
            }

            return html.ToString();
        }

        /// <summary>
        /// دریافت لیست فرصت های شغلی
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<ICareerOpportunityDto> GetList(Expression<Func<CareerOpportunity, bool>> filter = null,
            Func<IQueryable<CareerOpportunity>, IOrderedQueryable<CareerOpportunity>> orderBy = null,
            params Expression<Func<CareerOpportunity, object>>[] includes)
        {
            return _dataContext.CareerOpportunityRepository.GetList(filter, orderBy, includes).MapToListDto();
        }

        /// <summary>
        /// دریافت یک فرصت های شغلی
        /// </summary>
        /// <param name="careerOpportunityId"></param>
        /// <returns></returns>
        public ICareerOpportunityDto Get(Guid careerOpportunityId)
        {
            return _dataContext.CareerOpportunityRepository.GetByID(careerOpportunityId).MapToDto();
        }

        /// <summary>
        /// ثبت فرصت شغلی
        /// </summary>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Guid CreateNewCareerOpportunity(CareerOpportunityCreateCommand command, Guid userId)
        {
            var newCareerOpportunity = new CareerOpportunity()
            {
                Title = command.Title,
                Description = command.Description,
                CareerOpportunityImageGuid = command.CareerOpportunityImageGuid,
                CareerOpportunityType = command.CareerOpportunityType,
                IsActive = command.IsActive,
                CreatedBy = userId
            };
            Insert(newCareerOpportunity);
            Save();
            return newCareerOpportunity.Id;
        }

        /// <summary>
        /// ثبت فرصت شغلی
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(CareerOpportunity entity)
        {
            _dataContext.CareerOpportunityRepository.Insert(entity);
        }

        /// <summary>
        /// ویرایش فرصت شغلی
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        public void UpdateCareerOpportunity(CareerOpportunity entity, CareerOpportunityEditCommand command, Guid userId)
        {
            entity.Title = command.Title;
            entity.Description = command.Description;
            entity.CareerOpportunityImageGuid = command.CareerOpportunityImageGuid;
            entity.CareerOpportunityType = command.CareerOpportunityType;
            entity.ModifiedBy = userId;
            entity.ModifiedDate = DateTime.Now;
            entity.IsActive = command.IsActive;
            Update(entity);
            Save();
        }

        /// <summary>
        /// ویرایش فرصت شغلی
        /// </summary>
        /// <param name="entity"></param>
        public void Update(CareerOpportunity entity)
        {
            _dataContext.CareerOpportunityRepository.Update(entity);
        }

        /// <summary>
        /// حذف فرصت شغلی
        /// </summary>
        /// <param name="careerOpportunityId"></param>
        public void Delete(Guid careerOpportunityId)
        {
            _dataContext.CareerOpportunityRepository.Delete(careerOpportunityId);
        }

        public bool Any(Expression<Func<CareerOpportunity, bool>> filter = null)
        {
            return _dataContext.CareerOpportunityRepository.Any(filter);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
