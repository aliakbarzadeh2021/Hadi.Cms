using Hadi.Cms.ApplicationService.QueryModels;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Hadi.Cms.ApplicationService.Services
{
    public class ProvinceService
    {
        private DataContext _dataContext;

        public ProvinceService()
        {
            _dataContext = new DataContext();
        }

        public ProvinceDto Get(Expression<Func<Province, bool>> filter)
        {
            var province = _dataContext.ProvinceRepository.Get(filter);
            var provinceDto = MapToDto(province);
            return provinceDto;
        }

        public ProvinceDto GetById(object Id)
        {
            var province = _dataContext.ProvinceRepository.GetByID(Id);
            var provinceDto = MapToDto(province);
            return provinceDto;
        }

        public List<ProvinceDto> GetList(Expression<Func<Province, bool>> filter = null)
        {
            var provinces = _dataContext.ProvinceRepository.GetList(filter);
            var provincesDto = provinces.Select(MapToDto);
            return provincesDto.ToList();
        }

        public void Insert(Province model)
        {
            _dataContext.ProvinceRepository.Insert(model);
        }

        public void Update(Province model)
        {
            _dataContext.ProvinceRepository.Update(model);
        }

        public void Delete(Province model)
        {
            _dataContext.ProvinceRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.ProvinceRepository.Delete(Id);
        }

        public void DeleteRange(IEnumerable<Province> entities)
        {
            _dataContext.ProvinceRepository.DeleteRange(entities);
        }

        public IQueryable<Province> Where(Expression<Func<Province, bool>> filter)
        {
            return _dataContext.ProvinceRepository.Where(filter);
        }

        public bool Any(Expression<Func<Province, bool>> filter)
        {
            return _dataContext.ProvinceRepository.Any(filter);
        }

        public int Count(Expression<Func<Province, bool>> where = null)
        {
            return _dataContext.ProvinceRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }


        public static ProvinceDto MapToDto(Province model)
        {
            var provinceDto = new ProvinceDto
            {
                Id = model.Id,
                Name = model.Name
            };
            return provinceDto;
        }
    }
}
