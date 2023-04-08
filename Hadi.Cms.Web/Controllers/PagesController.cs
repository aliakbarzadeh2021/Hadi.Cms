using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Web.Utilities;
using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Razor.Parser;
using Hadi.Cms.Infrastructure.Helpers;
using Hadi.Cms.Model.Enums;

namespace Hadi.Cms.Web.Controllers
{
    public class PagesController : Controller
    {
        private readonly PageService _pageService;
        private readonly PageStatisticService _pageStatisticService;
        private readonly FormService _formService;
        private readonly FormFieldService _formFieldService;

        public PagesController()
        {
            _pageService = new PageService();
            _pageStatisticService = new PageStatisticService();
            _formService = new FormService();
            _formFieldService = new FormFieldService();
        }

        public ActionResult Details(string pageAlias)
        {
            try
            {
                var page = _pageService.Get(q => q.Alias == pageAlias);

                if (page == null)
                    return HttpNotFound("Page not found . ");


                if (page.Source.Contains("["))
                {
                    var variable = StringHelper.BetweenStrings(page.Source, "[", "]").ToLower();

                    #region Contact us form

                    if (variable.ToLower().Contains("contact"))
                    {
                        var form = _formService.Get(f => f.Name.ToLower().Contains("contact"));

                        if (form != null)
                            page.Source = page.Source.Replace($"[{variable}]", form.FormDataSource);
                        else
                            page.Source = page.Source.Replace($"[{variable}]", "");

                    }

                    #endregion

                }

                var userId = SessionData.Current.User != null ? SessionData.Current.User.Id : Guid.Empty;
                var statistic = new PageStatistic
                {
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    IsActive = true,
                    UserId = userId,
                    PageId = page.Id,
                    UserIpAddress = IpProvider.GetIpValue()
                };
                _pageStatisticService.Insert(statistic);
                _pageStatisticService.Save();
                return View(page);
            }

            catch (Exception ex)
            {
                return null;
            }
        }

    }
}