using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Web.Controllers;
using System.Web.Mvc;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    public class StatisticController : BaseController
    {
        public ActionResult Index()
        {
            var statisticService = new StatisticService();
            if (Request.Url != null)
            {
                ViewBag.AlexaRank = statisticService.GetAlexaRank(Request.Url.Authority);
                ViewBag.AlexaRankInCountry = statisticService.GetAlexaRankInCountry(Request.Url.Authority);
            }

            return View();
        }
    }
}