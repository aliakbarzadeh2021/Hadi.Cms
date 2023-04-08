using Hadi.Cms.ApplicationService.QueryModels;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Infrastructure.Utilities;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Interfaces;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;

namespace Hadi.Cms.ApplicationService.Services
{
    public class UserService
    {
        private static byte[] _picture;
        private static bool _pictureIsLoaded = false;
        private bool _pictureIsUpdated = false;
        private DataContext _dataContext;

        public UserService()
        {
            _dataContext = new DataContext();
        }

        public IUserDto Get(Expression<Func<User, bool>> filter, params Expression<Func<User, object>>[] includePropertys)
        {
            var user = _dataContext.UserRepository.Get(filter, includePropertys);
            return user.MapToDto();
        }

        public IUserDto GetById(object Id)
        {
            var user = _dataContext.UserRepository.GetByID(Id);
            return user.MapToDto();
        }

        public List<IUserDto> GetList(Expression<Func<User, bool>> filter = null, params Expression<Func<User, object>>[] includeProperty)
        {
            var users = _dataContext.UserRepository.GetList(filter, null, includeProperty);
            return users.MapToListDto();
        }

        public void Insert(User model)
        {
            _dataContext.UserRepository.Insert(model);
        }

        public void Update(User model)
        {
            _dataContext.UserRepository.Update(model);
        }

        public void Delete(User model)
        {
            _dataContext.UserRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.UserRepository.Delete(Id);
        }


        public void DeleteRange(IEnumerable<User> entities)
        {
            _dataContext.UserRepository.DeleteRange(entities);
        }

        public IQueryable<IUserDto> Where(Expression<Func<User, bool>> filter)
        {
            var users = _dataContext.UserRepository.Where(filter);
            var usersDto = users.Select(q => q.MapToDto());
            return usersDto;
        }

        public bool Any(Expression<Func<User, bool>> filter)
        {
            return _dataContext.UserRepository.Any(filter);
        }

        public int Count(Expression<Func<User, bool>> where = null)
        {
            return _dataContext.UserRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }


        public void Reload(User user)
        {
            _dataContext.Reload(user);
        }

        public UserAuthenticationResult Authenticate(string username, string password, out User user)
        {

            UserAuthenticationResult result = UserAuthenticationResult.InvalidUserNameOrPassword;
            string hash = CalculatePasswordHash(password);

            user = _dataContext.UserRepository.Get(u => u.UserName.ToLower() == username.ToLower() && u.PasswordHash == hash );
            //user = _dataContext.UserRepository.Get(u => u.UserName.ToLower() == username.ToLower() && u.PasswordHash == hash , u => u.Language);

            if (user != null)
            {
                if (!user.IsEnable)
                {
                    result = UserAuthenticationResult.UserDisabledByAdmin;
                }
                else
                {
                    result = UserAuthenticationResult.Successful;
                }
            }

            return result;
        }


        public bool CreateNewUser(string firstName, string lastName, bool IsEnable, string userName,
            string password, bool mostChangePassword)
        {
            if (userName != null && password != null)
            {
                var item = _dataContext.UserRepository.Get(u => u.UserName.ToLower() == userName.ToLower());
                if (item == null)
                {
                    var newUser = new User()
                    {
                        Id = Guid.NewGuid(),
                        FirstName = firstName,
                        LastName = lastName,
                        BuiltIn = false,
                        LanguageId = _dataContext.LanguageRepository.Get(q => q.CultureCode == "1033").Id,
                        IsDeleted = false,
                        MostChangePassword = mostChangePassword,
                        LoginRetryCount = 0,
                        IsEnable = IsEnable,
                        CreateDate = DateTime.Now,
                        UserName = userName
                    };
                    newUser.SetPassword(password);

                    Insert(newUser);
                    Save();

                    //set user role
                    var role = _dataContext.RoleRepository.Get(r => r.Name == "_authenticated_");
                    var newUserRole = new UserRole
                    {
                        UserId = newUser.Id,
                        RoleId = role.Id
                    };
                    _dataContext.UserRoleRepository.Insert(newUserRole);
                    Save();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static byte[] GetUserPicture(User user)
        {
            if (!_pictureIsLoaded)
            {
                _picture = BlobStorage.GetFileContent(user.Id);
            }
            if (_picture == null)
            {
                _picture = new AttachmentFileService().GetUserPictureContent(user.Id);
            }

            return _picture;
        }

        public static bool UserHasPicture(User user)
        {
            var res = false;
            if (user != null)
            {
                res = BlobStorage.Exists(user.Id); // Physical Storage
                if (!res)
                {
                    res = new AttachmentFileService().UserPictureExists(user.Id); // Binary Storage
                }
            }
            return res;
        }


        #region Private Method

        private static readonly string PasswordSalt = "993BA245D5E140F7AC59C95FDCBE4E22";
        private string CalculatePasswordHash(string password)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            return Convert.ToBase64String(algorithm.ComputeHash(Encoding.UTF8.GetBytes(password + PasswordSalt)));
        }

        private static UserDto MapToDto(User model)
        {
            var userDto = new UserDto
            {
                BuiltIn = model.BuiltIn,
                CreateDate = model.CreateDate,
                FirstName = model.FirstName,
                Id = model.Id,
                IsDeleted = model.IsDeleted,
                IsEnable = model.IsEnable,
                LastName = model.LastName,
                LanguageId = model.Language.Id,
                LoginRetryCount = model.LoginRetryCount,
                MostChangePassword = model.MostChangePassword,
                PasswordHash = model.PasswordHash,
                UserName = model.UserName
            };
            return userDto;
        }

        #endregion
    }
}
