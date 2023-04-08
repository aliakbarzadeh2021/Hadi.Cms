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
    public class UserRoleService
    {
        private DataContext _dataContext;

        public UserRoleService()
        {
            _dataContext = new DataContext();
        }

        public IUserRoleDto Get(Expression<Func<UserRole, bool>> filter)
        {
            var UserRole = _dataContext.UserRoleRepository.Get(filter);
            return UserRole.MapToDto();
        }

        public IUserRoleDto GetById(object Id)
        {
            var UserRole = _dataContext.UserRoleRepository.GetByID(Id);
            return UserRole.MapToDto();
        }

        public List<IUserRoleDto> GetList(Expression<Func<UserRole, bool>> filter = null)
        {
            var userRoles = _dataContext.UserRoleRepository.GetList(filter);
            return userRoles.MapToListDto();
        }

        public void Insert(UserRole model)
        {
            _dataContext.UserRoleRepository.Insert(model);
        }

        public void Update(UserRole model)
        {
            _dataContext.UserRoleRepository.Update(model);
        }

        public void Delete(UserRole model)
        {
            _dataContext.UserRoleRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.UserRoleRepository.Delete(Id);
        }

        public void DeleteRange(IEnumerable<UserRole> entities)
        {
            _dataContext.UserRoleRepository.DeleteRange(entities);
        }

        public IQueryable<IUserRoleDto> Where(Expression<Func<UserRole, bool>> filter)
        {
            var userRoles = _dataContext.UserRoleRepository.Where(filter);
            var userRolesDto = userRoles.Select(q => q.MapToDto());
            return userRolesDto;
        }

        public bool Any(Expression<Func<UserRole, bool>> filter)
        {
            return _dataContext.UserRoleRepository.Any(filter);
        }

        public int Count(Expression<Func<UserRole, bool>> where = null)
        {
            return _dataContext.UserRoleRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }


        public static UserRoleDto MapToDto(UserRole model)
        {
            var userRoleDto = new UserRoleDto
            {
                Id = model.Id,
                RoleId = model.RoleId,
                UserId = model.UserId
            };
            return userRoleDto;
        }

    }
}
