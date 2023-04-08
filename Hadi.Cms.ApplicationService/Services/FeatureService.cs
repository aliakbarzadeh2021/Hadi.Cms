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
    public class FeatureService
    {

        private DataContext _dataContext;

        public FeatureService()
        {
            _dataContext = new DataContext();
        }

        public IFeatureDto Get(Expression<Func<Feature, bool>> filter)
        {
            var feature = _dataContext.FeatureRepository.Get(filter);
            return feature.MapToDto();
        }

        public IFeatureDto GetById(object Id)
        {
            var feature = _dataContext.FeatureRepository.GetByID(Id);
            return feature.MapToDto();
        }

        public List<IFeatureDto> GetList(Expression<Func<Feature, bool>> filter = null)
        {
            var features = _dataContext.FeatureRepository.GetList(filter);
            return features.MapToListDto();
        }

        public void Insert(Feature model)
        {
            _dataContext.FeatureRepository.Insert(model);
        }

        public void Update(Feature model)
        {
            _dataContext.FeatureRepository.Update(model);
        }

        public void Delete(Feature model)
        {
            _dataContext.FeatureRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.FeatureRepository.Delete(Id);
        }

        public void DeleteRange(IEnumerable<Feature> entities)
        {
            _dataContext.FeatureRepository.DeleteRange(entities);
        }

        public IQueryable<IFeatureDto> Where(Expression<Func<Feature, bool>> filter)
        {
            var features = _dataContext.FeatureRepository.Where(filter);
            var featuresDto = features.Select(q => q.MapToDto());
            return featuresDto;
        }

        public bool Any(Expression<Func<Feature, bool>> filter)
        {
            return _dataContext.FeatureRepository.Any(filter);
        }

        public int Count(Expression<Func<Feature, bool>> where = null)
        {
            return _dataContext.FeatureRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }

        public static FeatureDto MapToDto(Feature model)
        {
            var featureDto = new FeatureDto
            {
                ActionName = model.ActionName,
                AreaName = model.AreaName,
                Attributes = model.Attributes,
                ControllerName = model.ControllerName,
                FeaturesName = model.FeaturesName,
                Id = model.Id
            };
            return featureDto;
        }

    }
}
