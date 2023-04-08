using Hadi.Cms.ApplicationService.QueryModels;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Hadi.Cms.ApplicationService.Services
{
    public class LanguageService
    {
        private DataContext _dataContext;

        public LanguageService()
        {
            _dataContext = new DataContext();
        }

        public ILanguageDto Get(Expression<Func<Model.Entities.Language, bool>> filter)
        {
            var language = _dataContext.LanguageRepository.Get(filter);
            return language.MapToDto();
        }

        public ILanguageDto GetById(object Id)
        {
            var language = _dataContext.LanguageRepository.GetByID(Id);
            return language.MapToDto();
        }

        public List<ILanguageDto> GetList(Expression<Func<Model.Entities.Language, bool>> filter = null)
        {
            var languages = _dataContext.LanguageRepository.GetList(filter);
            return languages.MapToListDto();
        }

        public void Insert(Model.Entities.Language model)
        {
            _dataContext.LanguageRepository.Insert(model);
        }

        public void Update(Model.Entities.Language model)
        {
            _dataContext.LanguageRepository.Update(model);
        }

        public void Delete(Model.Entities.Language model)
        {
            _dataContext.LanguageRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.LanguageRepository.Delete(Id);
        }

        public void DeleteRange(IEnumerable<Model.Entities.Language> entities)
        {
            _dataContext.LanguageRepository.DeleteRange(entities);
        }

        public IQueryable<ILanguageDto> Where(Expression<Func<Model.Entities.Language, bool>> filter)
        {
            var languages = _dataContext.LanguageRepository.Where(filter);
            var languagesDto = languages.Select(q => q.MapToDto());
            return languagesDto;
        }

        public bool Any(Expression<Func<Model.Entities.Language, bool>> filter)
        {
            return _dataContext.LanguageRepository.Any(filter);
        }

        public int Count(Expression<Func<Model.Entities.Language, bool>> where = null)
        {
            return _dataContext.LanguageRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }


        public static LanguageDto MapToDto(Model.Entities.Language model)
        {
            var languageDto = new LanguageDto
            {
                CultureCode = model.CultureCode,
                CultureName = model.CultureName,
                DisplayName = model.DisplayName,
                Id = model.Id,
                IsActive = model.IsActive,
                IsRtl = model.IsRtl,
                Name = model.Name
            };
            return languageDto;
        }

    }
}
