using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Web.Utilities;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Hadi.Cms.Web.Controllers
{
    public class NewsController : Controller
    {
        private NewsService _newsService;
        private PageStatisticService _PageStatisticService;

        public NewsController()
        {
            _newsService = new NewsService();
            _PageStatisticService = new PageStatisticService();
        }

        public ActionResult Index()
        {
            var newsList = _newsService.GetList(q => q.IsActive && !q.IsDeleted && q.IsPublished);
            return View(newsList);
        }

        public ActionResult Details(Guid id)
        {
            var news = _newsService.Get(q => q.IsActive && !q.IsDeleted && q.Id == id);

            var userId = SessionData.Current.User != null ? SessionData.Current.User.Id : Guid.Empty;

            var statistic = new PageStatistic
            {
                CreatedBy = userId,
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                IsActive = true,
                UserId = userId,
                PageId = id,
                UserIpAddress = IpProvider.GetIpValue()
            };
            _PageStatisticService.Insert(statistic);
            _PageStatisticService.Save();

            return View(news);
        }

        public ActionResult NewsPartial()
        {
            var news = _newsService.GetList(q => q.IsActive && !q.IsDeleted).OrderByDescending(q => q.CreatedDate).Take(4).ToList();
            return PartialView(news);
        }
    }
}