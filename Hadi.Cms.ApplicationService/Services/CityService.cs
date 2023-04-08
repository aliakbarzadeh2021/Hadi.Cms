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
    public class CityService
    {
        private DataContext _dataContext;

        public CityService()
        {
            _dataContext = new DataContext();
        }

        public ICityDto Get(Expression<Func<City, bool>> filter)
        {
            var city = _dataContext.CityRepository.Get(filter);
            return city.MapToDto();
        }

        public ICityDto GetById(object Id)
        {
            var city = _dataContext.CityRepository.GetByID(Id);
            return city.MapToDto();
        }

        public List<ICityDto> GetList(Expression<Func<City, bool>> filter = null)
        {
            var cities = _dataContext.CityRepository.GetList(filter);
            return cities.MapToListDto();
        }

        public void Insert(City model)
        {
            _dataContext.CityRepository.Insert(model);
        }

        public void Update(City model)
        {
            _dataContext.CityRepository.Update(model);
        }

        public void Delete(City model)
        {
            _dataContext.CityRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.CityRepository.Delete(Id);
        }

        public void DeleteRange(IEnumerable<City> entities)
        {
            _dataContext.CityRepository.DeleteRange(entities);
        }

        public IQueryable<ICityDto> Where(Expression<Func<City, bool>> filter)
        {
            var cities = _dataContext.CityRepository.Where(filter);
            var citiesDto = cities.Select(q => q.MapToDto());
            return citiesDto;
        }

        public bool Any(Expression<Func<City, bool>> filter)
        {
            return _dataContext.CityRepository.Any(filter);
        }

        public int Count(Expression<Func<City, bool>> where = null)
        {
            return _dataContext.CityRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }


        public static CityDto MapToDto(City model)
        {
            var cityDto = new CityDto
            {
                Id = model.Id,
                Name = model.Name,
                PrePhoneCode = model.PrePhoneCode,
                ProvinceId = model.ProvinceId
            };
            return cityDto;
        }

    }
}
