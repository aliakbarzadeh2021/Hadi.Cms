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
    public class NewsService
    {
        private DataContext _dataContext;

        public NewsService()
        {
            _dataContext = new DataContext();
        }

        public INewsDto Get(Expression<Func<News, bool>> filter)
        {
            var news = _dataContext.NewsRepository.Get(filter);
            return news.MapToDto();
        }

        public INewsDto GetById(object Id)
        {
            var news = _dataContext.NewsRepository.GetByID(Id);
            return news.MapToDto();
        }

        public List<INewsDto> GetList(Expression<Func<News, bool>> filter = null , Func<IQueryable<News>,IOrderedQueryable<News>> orderBy = null , params Expression<Func<News,object>>[] includes)
        {
            var newsList = _dataContext.NewsRepository.GetList(filter , orderBy , includes)
                .OrderByDescending(q => q.CreatedDate).ToList();
            return newsList.MapToListDto();
        }

        public void Insert(News model)
        {
            _dataContext.NewsRepository.Insert(model);
        }

        public void Update(News model)
        {
            _dataContext.NewsRepository.Update(model);
        }

        public void Delete(News model)
        {
            _dataContext.NewsRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.NewsRepository.Delete(Id);
        }


        public void DeleteRange(IEnumerable<News> entities)
        {
            _dataContext.NewsRepository.DeleteRange(entities);
        }

        public IQueryable<INewsDto> Where(Expression<Func<News, bool>> filter)
        {
            var newsList = _dataContext.NewsRepository.Where(filter);
            var newsListDto = newsList.Select(q => q.MapToDto());
            return newsListDto;
        }

        public bool Any(Expression<Func<News, bool>> filter)
        {
            return _dataContext.NewsRepository.Any(filter);
        }

        public int Count(Expression<Func<News, bool>> where = null)
        {
            return _dataContext.NewsRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }


        public List<INewsDto> GetTop_N_News(int n)
        {
            var news = GetList(x => x.IsPublished).OrderByDescending(o => o.ReleaseDate).Take(n).ToList();
            return news;
        }


        public static NewsDto MapToDto(News model)
        {
            var newsDto = new NewsDto
            {
                Id = model.Id,
                Image = model.Image,
                IsHotLink = model.IsHotLink,
                IsLiveNews = model.IsLiveNews,
                IsMainTitr = model.IsMainTitr,
                IsPublished = model.IsPublished,
                MainTitrImage = model.MainTitrImage,
                ReleaseDate = model.ReleaseDate,
                RuTitr = model.RuTitr,
                ShowPriorityDate = model.ShowPriorityDate,
                Source = model.Source,
                SubTitle = model.SubTitle,
                ThumbnailImage = model.ThumbnailImage,
                Title = model.Title,
                WithFilm = model.WithFilm,
                WithImage = model.WithImage,
                WithVoice = model.WithVoice
            };
            return newsDto;
        }
    }
}
