using Hadi.Cms.ApplicationService.CommandModels;
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
    /// <summary>
    /// سرویس نویسنده
    /// </summary>
    public class AuthorService
    {
        private readonly DataContext _dataContext;

        public AuthorService()
        {
            _dataContext = new DataContext();
        }

        ~AuthorService()
        {
            _dataContext?.Dispose();
        }

        /// <summary>
        /// دریافت لیست نویسندگان
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public List<IAuthorDto> GetList(Expression<Func<Author, bool>> filter = null, Func<IQueryable<Author>, IOrderedQueryable<Author>> orderBy = null, params Expression<Func<Author, object>>[] includes)
        {
            var authors = _dataContext.AuthorRepository.GetList(filter, orderBy, includes);
            return authors.MapToListDto();
        }

        /// <summary>
        /// دریافت نویسنده بر اساس شناسه
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        public IAuthorDto GetById(Guid authorId)
        {
            var author = _dataContext.AuthorRepository.GetByID(authorId);
            return author.MapToDto();
        }

        /// <summary>
        /// ثبت نویسنده جدید
        /// </summary>
        /// <param name="author"></param>
        public void Insert(Author author)
        {
            _dataContext.AuthorRepository.Insert(author);
        }

        /// <summary>
        /// ثبت نویسنده جدید
        /// </summary>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        public Guid CreateNewAuthor(AuthorCreateCommand command, Guid userId)
        {
            var newAuthor = new Author
            {
                FullName = command.FullName,
                AuthorImageGuid = command.AuthorImageGuid,
                InstagramAddress = string.IsNullOrEmpty(command.InstagramAddress) ? "#" : command.InstagramAddress,
                TelegramAddress = string.IsNullOrEmpty(command.TelegramAddress) ? "#" : command.TelegramAddress,
                LinkedInAddress = string.IsNullOrEmpty(command.LinkedInAddress) ? "#" : command.LinkedInAddress,
                CreatedBy = userId
            };

            Insert(newAuthor);
            Save();

            return newAuthor.Id;
        }

        /// <summary>
        /// ویرایش اطلاعات نویسنده
        /// </summary>
        /// <param name="author"></param>
        public void Update(Author author)
        {
            _dataContext.AuthorRepository.Update(author);
        }

        /// <summary>
        /// ویرایش اطلاعات نویسنده
        /// </summary>
        /// <param name="author"></param>
        /// <param name="command"></param>
        /// <param name="userId"></param>
        public void EditAuthorInformation(Author author, AuthorEditCommand command, Guid userId)
        {
            author.FullName = command.FullName;
            author.AuthorImageGuid = command.AuthorImageGuid;
            author.InstagramAddress = string.IsNullOrEmpty(command.InstagramAddress) ? "#" : command.InstagramAddress;
            author.TelegramAddress = string.IsNullOrEmpty(command.TelegramAddress) ? "#" : command.TelegramAddress;
            author.LinkedInAddress = string.IsNullOrEmpty(command.LinkedInAddress) ? "#" : command.LinkedInAddress;
            author.ModifiedBy = userId;
            author.ModifiedDate = DateTime.Now;

            Update(author);
            Save();
        }

        /// <summary>
        /// حذف نویسنده
        /// </summary>
        /// <param name="authorId"></param>
        public void DeleteById(Guid authorId)
        {
            _dataContext.AuthorRepository.Delete(authorId);
        }

        public bool Any(Expression<Func<Author, bool>> filter = null)
        {
            return _dataContext.AuthorRepository.Any(filter);
        }

        public void Save()
        {
            _dataContext.Save();
        }
    }
}
