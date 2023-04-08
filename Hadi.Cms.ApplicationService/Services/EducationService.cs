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
    public class EducationService
    {
        private DataContext _dataContext;

        public EducationService()
        {
            _dataContext = new DataContext();
        }

        public IEducationDto Get(Expression<Func<Education, bool>> filter)
        {
            var education = _dataContext.EducationRepository.Get(filter);
            return education.MapToDto();
        }

        public IEducationDto GetById(object Id)
        {
            var education = _dataContext.EducationRepository.GetByID(Id);
            return education.MapToDto();
        }

        public List<IEducationDto> GetList(Expression<Func<Education, bool>> filter = null)
        {
            var educations = _dataContext.EducationRepository.GetList(filter);
            return educations.MapToListDto();
        }

        public void Insert(Education model)
        {
            _dataContext.EducationRepository.Insert(model);
        }

        public void Update(Education model)
        {
            _dataContext.EducationRepository.Update(model);
        }

        public void Delete(Education model)
        {
            _dataContext.EducationRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.EducationRepository.Delete(Id);
        }

        public void DeleteRange(IEnumerable<Education> entities)
        {
            _dataContext.EducationRepository.DeleteRange(entities);
        }

        public IQueryable<IEducationDto> Where(Expression<Func<Education, bool>> filter)
        {
            var educations = _dataContext.EducationRepository.Where(filter);
            var educationsDto = educations.Select(q => q.MapToDto());
            return educationsDto;
        }

        public bool Any(Expression<Func<Education, bool>> filter)
        {
            return _dataContext.EducationRepository.Any(filter);
        }

        public int Count(Expression<Func<Education, bool>> where = null)
        {
            return _dataContext.EducationRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }


        public static EducationDto MapToDto(Education model)
        {
            var educationDto = new EducationDto
            {
                Id = model.Id,
                Title = model.Title
            };
            return educationDto;
        }
    }
}
