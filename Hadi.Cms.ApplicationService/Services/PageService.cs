using Hadi.Cms.Language.Resources;
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
    public class PageService
    {
        private DataContext _dataContext;

        public PageService()
        {
            _dataContext = new DataContext();
        }



        public IPageDto Get(Expression<Func<Page, bool>> filter)
        {
            var page = _dataContext.PageRepository.Get(filter);
            return page.MapToDto();
        }

        public IPageDto GetById(object Id)
        {
            var page = _dataContext.PageRepository.GetByID(Id);
            return page.MapToDto();
        }

        public List<IPageDto> GetList(Expression<Func<Page, bool>> filter = null)
        {
            var pages = _dataContext.PageRepository.GetList(filter);
            return pages.MapToListDto();
        }

        public void Insert(Page model)
        {
            _dataContext.PageRepository.Insert(model);
        }

        public void Update(Page model)
        {
            _dataContext.PageRepository.Update(model);
        }

        public void Delete(Page model)
        {
            _dataContext.PageRepository.Delete(model);
        }

        public void DeleteById(Guid Id)
        {
            _dataContext.PageRepository.Delete(Id);
        }

        public void DeleteRange(IEnumerable<Page> entities)
        {
            _dataContext.PageRepository.DeleteRange(entities);
        }

        public IQueryable<IPageDto> Where(Expression<Func<Page, bool>> filter)
        {
            var pages = _dataContext.PageRepository.Where(filter);
            var pagesDto = pages.Select(q => q.MapToDto());
            return pagesDto;
        }

        public bool Any(Expression<Func<Page, bool>> filter)
        {
            return _dataContext.PageRepository.Any(filter);
        }

        public int Count(Expression<Func<Page, bool>> where = null)
        {
            return _dataContext.PageRepository.Count(where);
        }

        public void Save()
        {
            _dataContext.Save();
        }


        public List<Page> GetSreachedPages(string pageFilter, string text, bool? justAccepted = false)
        {
            var result = new List<Page>();

            var searchedPages = new List<Page>();
            if (text.Contains("-"))
            {
                searchedPages = _dataContext.PageRepository.GetList(c => !c.IsDeleted && c.Title.ToLower().Contains(text.ToLower()));
            }
            else
            {
                searchedPages.AddRange(_dataContext.PageRepository.GetList(c =>
                   !c.IsDeleted && (c.Title.ToLower().Contains(text.ToLower()) ||
                                  c.Description.ToLower().Contains(text.ToLower()) ||
                                  c.Keywords.ToLower().Contains(text.ToLower()))).Distinct());
            }

            if (pageFilter == "New")
            {
                if (justAccepted == true)
                {
                    result.AddRange(searchedPages.Where(c => c.Accepted).OrderByDescending(o => o.CreatedDate).ToList());
                }
                else
                {
                    result.AddRange(searchedPages.OrderByDescending(o => o.CreatedDate).ToList());
                }
            }
            else if (pageFilter == "Like")
            {
                var searchedPageIDs = searchedPages.Select(x => x.Id).ToList();
                if (justAccepted == true)
                {
                    //result.AddRange(db.Page.Where(c => searchedPageIDs.Contains(c.PageID)).Where(c => c.Accepted).OrderByDescending(o => o.PageRating.Average(a => a.RateValue)).ThenByDescending(o => o.PageRating.Count).ToList());
                    result.AddRange(_dataContext.PageRepository.Where(c => searchedPageIDs.Contains(c.Id)).Where(c => c.Accepted).ToList());
                }
                else
                {
                    result.AddRange(_dataContext.PageRepository.Where(c => searchedPageIDs.Contains(c.Id)).ToList());
                }
            }
            else if (pageFilter == "Update")
            {
                if (justAccepted == true)
                {
                    result.AddRange(searchedPages.Where(c => c.Accepted).OrderByDescending(o => o.ModifiedDate).ToList());
                }
                else
                {
                    result.AddRange(searchedPages.OrderByDescending(o => o.ModifiedDate).ToList());
                }
            }

            return result;
        }

        public string GetPageLink(Guid pageGuid)
        {
            var link = "";

            var page = _dataContext.PageRepository.Get(d => d.Id == pageGuid);
            if (page != null)
            {
                link = "/view/pages?pageId=" + page.Id;
            }

            return link;
        }

        public string GetPageDescription(Guid pageId)
        {
            var description = Strings.Global_KeywordMetaTag;

            if (pageId != Guid.NewGuid())
            {
                var page = _dataContext.PageRepository.Get(d => d.Id == pageId);
                if (page != null)
                {
                    description = page.Description;
                }
            }

            return description;
        }

        public string GetPageKeywords(Guid pageId)
        {
            var keywords = Strings.Global_KeywordMetaTag;

            if (pageId != Guid.NewGuid())
            {
                var page = _dataContext.PageRepository.Get(d => d.Id == pageId);
                if (page != null)
                {
                    keywords = page.Keywords;
                }
            }

            return keywords;
        }

        public void SetPageStatistics(Guid pageId, Guid userId, string userIp)
        {
            var page = _dataContext.PageRepository.Get(d => d.Id == pageId);
            if (page != null)
            {
                var newStatistics = new PageStatistic()
                {
                    PageId = page.Id,
                    UserId = userId,
                    UserIpAddress = userIp
                };
                _dataContext.PageStatisticRepository.Insert(newStatistics);
                _dataContext.Save();
            }
        }

        public int GetPageViewCount(Guid pageId)
        {
            var viewCount = _dataContext.PageStatisticRepository.Count(d => d.Id == pageId);
            return viewCount;
        }

        //public List<Page> GetMostViewedPageTop_N(int n)
        //{
        //    var pages = _dataContext.PageRepository.Where(c => !c.IsDeleted).OrderByDescending(o => o.CreatedDate).OrderByDescending(o => o.PageStatistics.Count).ToArray().Take(n).ToList();
        //    return pages;
        //}


    }
}
