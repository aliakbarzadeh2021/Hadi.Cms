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
    public class MenuService
    {
        private DataContext _dataContext;

        public MenuService()
        {
            _dataContext = new DataContext();
        }

        public IMenuDto Get(Expression<Func<Menu, bool>> filter)
        {
            var menu = _dataContext.MenuRepository.Get(filter);
            return menu.MapToDto();
        }

        public IMenuDto GetById(object Id)
        {
            var menu = _dataContext.MenuRepository.GetByID(Id);
            return menu.MapToDto();
        }

        public List<IMenuDto> GetList(Expression<Func<Menu, bool>> filter = null)
        {
            var menus = _dataContext.MenuRepository.GetList(filter);
            return menus.MapToListDto();
        }

        public void Insert(Menu model)
        {
            _dataContext.MenuRepository.Insert(model);
        }

        public void Update(Menu model)
        {
            _dataContext.MenuRepository.Update(model);
        }

        public void Delete(Menu model)
        {
            _dataContext.MenuRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.MenuRepository.Delete(Id);
        }

        public void DeleteRange(IEnumerable<Menu> entities)
        {
            _dataContext.MenuRepository.DeleteRange(entities);
        }

        public IQueryable<Menu> Where(Expression<Func<Menu, bool>> filter)
        {
            var menus = _dataContext.MenuRepository.Where(filter);
            return menus;
        }

        public bool Any(Expression<Func<Menu, bool>> filter)
        {
            return _dataContext.MenuRepository.Any(filter);
        }

        public int Count(Expression<Func<Menu, bool>> where = null)
        {
            return _dataContext.MenuRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }

    }
}
