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
    public class UserProfileService
    {
        private DataContext _dataContext;

        public UserProfileService()
        {
            _dataContext = new DataContext();
        }

        public IUserProfileDto Get(Expression<Func<UserProfile, bool>> filter)
        {
            var userProfile = _dataContext.UserProfileRepository.Get(filter);
            return userProfile.MapToDto();
        }

        public IUserProfileDto GetById(object Id)
        {
            var userProfile = _dataContext.UserProfileRepository.GetByID(Id);
            return userProfile.MapToDto();
        }

        public List<IUserProfileDto> GetList(Expression<Func<UserProfile, bool>> filter = null)
        {
            var userProfiles = _dataContext.UserProfileRepository.GetList(filter);
            return userProfiles.MapToListDto();
        }

        public void Insert(UserProfile model)
        {
            _dataContext.UserProfileRepository.Insert(model);
        }

        public void Update(UserProfile model)
        {
            _dataContext.UserProfileRepository.Update(model);
        }

        public void Delete(UserProfile model)
        {
            _dataContext.UserProfileRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.UserProfileRepository.Delete(Id);
        }


        public void DeleteRange(IEnumerable<UserProfile> entities)
        {
            _dataContext.UserProfileRepository.DeleteRange(entities);
        }

        public IQueryable<IUserProfileDto> Where(Expression<Func<UserProfile, bool>> filter)
        {
            var userProfiles = _dataContext.UserProfileRepository.Where(filter);
            var userProfilesDto = userProfiles.Select(q => q.MapToDto());
            return userProfilesDto;
        }

        public bool Any(Expression<Func<UserProfile, bool>> filter)
        {
            return _dataContext.UserProfileRepository.Any(filter);
        }

        public int Count(Expression<Func<UserProfile, bool>> where = null)
        {
            return _dataContext.UserProfileRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }


        public static UserProfileDto MapToDto(UserProfile model)
        {
            var userProfileDto = new UserProfileDto
            {
                BirthDate = model.BirthDate,
                CityId = model.CityId,
                EducationId = model.EducationId,
                EmailAddress = model.EmailAddress,
                Gender = model.Gender.Value,
                Id = model.Id,
                ImageAttachmentGuid = model.ImageAttachmentGuid,
                MobileNumber = model.MobileNumber,
                MobileNumberIsValid = model.MobileNumberIsValid,
                NationalCode = model.NationalCode,
                PhoneNumber = model.PhoneNumber,
                UserId = model.UserId
            };
            return userProfileDto;
        }
    }
}
