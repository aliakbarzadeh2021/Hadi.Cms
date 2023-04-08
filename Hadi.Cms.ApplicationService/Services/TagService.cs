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
    public class TagService
    {
        private DataContext _dataContext;

        public TagService()
        {
            _dataContext = new DataContext();
        }

        public ITagDto Get(Expression<Func<Tag, bool>> filter)
        {
            var tag = _dataContext.TagRepository.Get(filter);
            return tag.MapToDto();
        }

        public ITagDto GetById(object Id)
        {
            var tag = _dataContext.TagRepository.GetByID(Id);
            return tag.MapToDto();
        }

        public List<ITagDto> GetList(Expression<Func<Tag, bool>> filter = null, Func<IQueryable<Tag>, IOrderedQueryable<Tag>> orderBy = null, params Expression<Func<Tag, object>>[] includes)
        {
            var tags = _dataContext.TagRepository.GetList(filter, orderBy, includes);
            return tags.MapToListDto();
        }

        public void Insert(Tag model)
        {
            _dataContext.TagRepository.Insert(model);
        }

        public void Update(Tag model)
        {
            _dataContext.TagRepository.Update(model);
        }

        public void Delete(Tag model)
        {
            _dataContext.TagRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.TagRepository.Delete(Id);
        }

        public void DeleteRange(IEnumerable<Tag> entities)
        {
            _dataContext.TagRepository.DeleteRange(entities);
        }

        public IQueryable<ITagDto> Where(Expression<Func<Tag, bool>> filter)
        {
            var tags = _dataContext.TagRepository.Where(filter);
            var tagsDto = tags.Select(q => q.MapToDto());
            return tagsDto;
        }

        public bool Any(Expression<Func<Tag, bool>> filter)
        {
            return _dataContext.TagRepository.Any(filter);
        }

        public int Count(Expression<Func<Tag, bool>> where = null)
        {
            return _dataContext.TagRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }

    }
}
