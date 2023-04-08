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
    public class RoleService
    {
        private DataContext _dataContext;

        public RoleService()
        {
            _dataContext = new DataContext();
        }

        public IRoleDto Get(Expression<Func<Role, bool>> filter)
        {
            var role = _dataContext.RoleRepository.Get(filter);
            return role.MapToDto();
        }

        public IRoleDto GetById(object Id)
        {
            var role = _dataContext.RoleRepository.GetByID(Id);
            return role.MapToDto();
        }

        public List<IRoleDto> GetList(Expression<Func<Role, bool>> filter = null, Func<IQueryable<Role>, IOrderedQueryable<Role>> orderBy = null, params Expression<Func<Role, object>>[] includes)
        {
            var roles = _dataContext.RoleRepository.GetList(filter, orderBy, includes);
            return roles.MapToListDto();
        }

        public void Insert(Role model)
        {
            _dataContext.RoleRepository.Insert(model);
        }

        public void Update(Role model)
        {
            _dataContext.RoleRepository.Update(model);
        }

        public void Delete(Role model)
        {
            _dataContext.RoleRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.RoleRepository.Delete(Id);
        }


        public void DeleteRange(IEnumerable<Role> entities)
        {
            _dataContext.RoleRepository.DeleteRange(entities);
        }

        public IQueryable<IRoleDto> Where(Expression<Func<Role, bool>> filter)
        {
            var roles = _dataContext.RoleRepository.Where(filter);
            var rolesDto = roles.Select(q => q.MapToDto());
            return rolesDto;
        }

        public bool Any(Expression<Func<Role, bool>> filter)
        {
            return _dataContext.RoleRepository.Any(filter);
        }

        public int Count(Expression<Func<Role, bool>> where = null)
        {
            return _dataContext.RoleRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }



        public static RoleDto MapToDto(Role model)
        {
            var roleDto = new RoleDto
            {
                DisplayName = model.DisplayName,
                Id = model.Id,
                IsActive = model.IsActive,
                Name = model.Name
            };
            return roleDto;
        }
    }
}
