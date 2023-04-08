using Hadi.Cms.ApplicationService.QueryModels;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Hadi.Cms.ApplicationService.Services
{
    public class SettingService
    {
        private DataContext _dataContext;

        public SettingService()
        {
            _dataContext = new DataContext();
        }

        public SettingDto Get(Expression<Func<Setting, bool>> filter)
        {
            var setting = _dataContext.SettingRepository.Get(filter);
            var settingDto = MapToDto(setting);
            return settingDto;
        }

        public SettingDto GetById(object Id)
        {
            var setting = _dataContext.SettingRepository.GetByID(Id);
            var settingDto = MapToDto(setting);
            return settingDto;
        }

        public List<SettingDto> GetList(Expression<Func<Setting, bool>> filter = null)
        {
            var settings = _dataContext.SettingRepository.GetList(filter);
            var settingsDto = settings.Select(MapToDto);
            return settingsDto.ToList();
        }

        public void Insert(Setting model)
        {
            _dataContext.SettingRepository.Insert(model);
        }

        public void Update(Setting model)
        {
            _dataContext.SettingRepository.Update(model);
        }

        public void Delete(Setting model)
        {
            _dataContext.SettingRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.SettingRepository.Delete(Id);
        }


        public void DeleteRange(IEnumerable<Setting> entities)
        {
            _dataContext.SettingRepository.DeleteRange(entities);
        }


        public IQueryable<Setting> Where(Expression<Func<Setting, bool>> filter)
        {
            return _dataContext.SettingRepository.Where(filter);
        }

        public bool Any(Expression<Func<Setting, bool>> filter)
        {
            return _dataContext.SettingRepository.Any(filter);
        }

        public int Count(Expression<Func<Setting, bool>> where = null)
        {
            return _dataContext.SettingRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }



        public static SettingDto MapToDto(Setting model)
        {
            var settingDto = new SettingDto
            {
                Id = model.Id,
                Key = model.Key,
                Value = model.Value
            };
            return settingDto;
        }

    }
}
