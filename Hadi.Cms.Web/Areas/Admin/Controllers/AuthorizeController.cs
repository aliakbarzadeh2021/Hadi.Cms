using Hadi.Cms.ApplicationService.Services;
using Hadi.Cms.Model.Entities;
using Hadi.Cms.Model.Mappings.Mappers;
using Hadi.Cms.Web.Controllers;
using System;
using System.Web.Mvc;

namespace Hadi.Cms.Web.Areas.Admin.Controllers
{
    public class AuthorizeController : BaseController
    {
        private RoleService _roleService;
        private RoleFeatureService _roleFeatureService;
        private FeatureService _featureService;


        public AuthorizeController()
        {
            _roleFeatureService = new RoleFeatureService();
            _featureService = new FeatureService();
            _roleService = new RoleService();
        }


        public ActionResult Index(Guid? roleId)
        {
            var features = _featureService.GetList().MaptoEntities();
            ViewBag.Features = features;

            var roles = _roleService.GetList().MaptoEntities();
            ViewBag.Roles = roles;

            var roleFeature = _roleFeatureService.GetList(q => q.RoleId == roleId);
            ViewBag.RoleFeature = roleFeature;

            if (roleId != null)
            {
                ViewBag.RoleId = roleId;
            }

            return View();
        }

        [HttpPost]
        public ActionResult SetFeatureToRole(Guid FeatureId, Guid RoleId)
        {
            string Type = "";
            var item = _roleFeatureService.Get(r => r.RoleId == RoleId && r.FeatureId == FeatureId);
            if (item == null)
            {
                var newItem = new RoleFeature()
                {
                    FeatureId = FeatureId,
                    RoleId = RoleId
                };
                _roleFeatureService.Insert(newItem);
                Type = "add";
            }
            else
            {
                _roleFeatureService.DeleteById(item.Id);
                Type = "remove";
            }
            _roleFeatureService.Save();
            return Json(new
            {
                type = Type,
                featureId = FeatureId
            }, JsonRequestBehavior.AllowGet);
        }

    }
}