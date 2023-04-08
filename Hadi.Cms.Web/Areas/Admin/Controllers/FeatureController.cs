using PagedList;
using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Infrastructure.Types;
using Hadi.Cms.Log.EventLoger;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Web.Controllers;
using Hadi.Cms.Web.Utilities;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    public class FeatureController : BaseController
    {
        private FeatureService _featureService;
        private RoleFeatureService _roleFeatureService;
        private EventLoger _eventLoger;

        public FeatureController()
        {
            _featureService = new FeatureService();
            _roleFeatureService = new RoleFeatureService();
            _eventLoger = new EventLoger();
        }

        public ActionResult Index(int pageNumber = 1)
        {
            int pageSize = 20;

            var features = _featureService.GetList().OrderBy(q => q.AreaName).ToList();

            var pagedListFeatures = features.ToPagedList(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;

            return View(pagedListFeatures);
        }

        public ActionResult UpdateFeature()
        {
            Assembly asm = Assembly.GetAssembly(typeof(Hadi.Cms.Web.MvcApplication));
            var areaControllerActionList = asm.GetTypes()
            .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
            .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
            .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
            .Select(x => new { Area = (x.DeclaringType.FullName.Contains("Areas") ? x.DeclaringType.FullName.Substring(x.DeclaringType.FullName.IndexOf("Areas.") + 6, (x.DeclaringType.FullName.IndexOf(".Controllers") - (x.DeclaringType.FullName.IndexOf("Areas.") + 6))) : ""), Controller = x.DeclaringType.Name, Action = x.Name, ReturnType = x.ReturnType.Name, Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))) })
            .OrderBy(x => x.Area).ThenBy(x => x.Controller).ThenBy(x => x.Action).ToList();

            var availableFeatures = _featureService.GetList().OrderBy(q => q.AreaName).ToList();

            foreach (var newItem in areaControllerActionList)
            {
                bool exist = false;
                foreach (var availableItem in availableFeatures)
                {
                    if (availableItem.AreaName == newItem.Area && availableItem.ControllerName == newItem.Controller && availableItem.ActionName == newItem.Action)
                    {
                        exist = true;
                        break;
                    }
                }

                if (!exist)
                {
                    var newFeature = new Feature()
                    {
                        AreaName = newItem.Area,
                        ControllerName = newItem.Controller,
                        ActionName = newItem.Action,
                        Attributes = newItem.Attributes,
                        FeaturesName = ""
                    };
                    _featureService.Insert(newFeature);
                    _eventLoger.LogEvent(EventType.Information, SessionData.Current.User.Id, SessionData.Current.User.UserName, "FeatureController", "Create", "Success Create Feature", HttpContext.Request.UserHostAddress, HttpContext.Request.UserAgent);
                }
            }
            _featureService.Save();

            // پاک کردن موارد حذف شده از دیتابیس
            foreach (var availableItem in availableFeatures)
            {
                bool exist = false;
                foreach (var newItem in areaControllerActionList)
                {
                    if (availableItem.AreaName == newItem.Area && availableItem.ControllerName == newItem.Controller && availableItem.ActionName == newItem.Action)
                    {
                        exist = true;
                        break;
                    }
                }

                if (!exist)
                {
                    _featureService.DeleteById(availableItem.Id);
                    var roleFeatures = _roleFeatureService.GetList(r => r.FeatureId == availableItem.Id);
                    if (roleFeatures != null && roleFeatures.Count() > 0)
                    {
                        _roleFeatureService.DeleteRange(roleFeatures);
                        _roleFeatureService.Save();
                    }

                }
            }
            _featureService.Save();

            ViewBag.Message = "بروز رسانی با موفقیت انجام شد.";
            ViewBag.Success = "1";

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetFeatureName(Guid featureId, string featureName)
        {
            var feature = _featureService.Get(q => q.Id == featureId).MaptoEntity();
            if (feature != null)
            {
                feature.FeaturesName = featureName;
                _featureService.Update(feature);
                _featureService.Save();
            }
            return Json("OK", JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetFeature(Guid FeatureId)
        {
            var feature = _featureService.GetById(FeatureId).MaptoEntity();
            return Json(feature, JsonRequestBehavior.AllowGet);
        }

    }
}