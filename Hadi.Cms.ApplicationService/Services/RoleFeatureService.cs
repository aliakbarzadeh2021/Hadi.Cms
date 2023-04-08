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
    public class RoleFeatureService
    {
        private DataContext _dataContext;

        public RoleFeatureService()
        {
            _dataContext = new DataContext();
        }

        public IRoleFeatureDto Get(Expression<Func<RoleFeature, bool>> filter)
        {
            var roleFeature = _dataContext.RoleFeatureRepository.Get(filter);
            return roleFeature.MapToDto();
        }

        public IRoleFeatureDto GetById(object Id)
        {
            var roleFeature = _dataContext.RoleFeatureRepository.GetByID(Id);
            return roleFeature.MapToDto();
        }

        public List<IRoleFeatureDto> GetList(Expression<Func<RoleFeature, bool>> filter = null)
        {
            var roleFeatures = _dataContext.RoleFeatureRepository.GetList(filter).ToList();
            return roleFeatures.MapToListDto();
        }

        public void Insert(RoleFeature model)
        {
            _dataContext.RoleFeatureRepository.Insert(model);
        }

        public void Update(RoleFeature model)
        {
            _dataContext.RoleFeatureRepository.Update(model);
        }

        public void Delete(RoleFeature model)
        {
            _dataContext.RoleFeatureRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.RoleFeatureRepository.Delete(Id);
        }

        public void DeleteRange(IEnumerable<IRoleFeatureDto> entities)
        {
            var roleFeatures = entities.ToList().MaptoEntities();
            _dataContext.RoleFeatureRepository.DeleteRange(roleFeatures);
        }

        public IQueryable<IRoleFeatureDto> Where(Expression<Func<RoleFeature, bool>> filter)
        {
            var roleFeatures = _dataContext.RoleFeatureRepository.Where(filter);
            var roleFeaturesDto = roleFeatures.Select(q => q.MapToDto());
            return roleFeaturesDto;
        }

        public bool Any(Expression<Func<RoleFeature, bool>> filter)
        {
            return _dataContext.RoleFeatureRepository.Any(filter);
        }

        public int Count(Expression<Func<RoleFeature, bool>> where = null)
        {
            return _dataContext.RoleFeatureRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }



        public static RoleFeatureDto MapToDto(RoleFeature model)
        {
            var roleFeatureDto = new RoleFeatureDto
            {
                FeatureId = model.FeatureId,
                Id = model.Id,
                RoleId = model.RoleId
            };
            return roleFeatureDto;
        }

    }
}
