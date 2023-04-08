using System.Collections.Generic;
using Hadi.Cms.ApplicationService.Services;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Hadi.Cms.Model.Mappings.Interfaces;

namespace Hadi.Cms.Web.Controllers
{
    public class MenusController : Controller
    {
        private readonly MenuService _menuService;
        private readonly LanguageService _languageService;


        public MenusController()
        {
            _menuService = new MenuService();
            _languageService = new LanguageService();
        }

        /// <summary>
        /// دریافت منو
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetMenu()
        {
            var culture = Thread.CurrentThread.CurrentCulture.Name;
            var language = _languageService.Get(q => q.IsActive && q.CultureName == culture);
            var menus = _menuService.GetList(m => !m.IsDeleted && m.IsActive && m.LanguageId == language.Id);

            var parentMenus = menus.Where(m => m.ParentId == null).ToList();
            var megaMenu = new List<IMenuDto>();

            foreach (var parentMenu in parentMenus)
                megaMenu.AddRange(menus.Where(m => m.ParentId == parentMenu.Id && !m.IsSideBar));

            return Json(new
            {
                #region Navbar

                Navbar = parentMenus.Select(p => new
                {
                    Id = p.Id,
                    Title = p.Title,
                    Link = p.Link,
                    OrderId = p.OrderId,
                    HasMegaMenu = megaMenu.Any(m => m.ParentId == p.Id)
                }).OrderBy(p => p.OrderId).ToList(),

                #endregion

                #region Mega menu
                MegaMenu = megaMenu.Select(m => new
                {
                    Id = m.Id,
                    Title = m.Title,
                    Color = m.Color,
                    Link = m.Link,
                    OrderId = m.OrderId,
                    MegaMenuItem = menus.Where(me => me.ParentId == m.Id && !me.IsSideBar).Select(me => new
                    {
                        Id = me.Id,
                        Title = me.Title,
                        Link = me.Link,
                        OrderId = me.OrderId
                    }).OrderBy(me => me.OrderId).ToList()
                }).OrderBy(m => m.OrderId).ToList(),

                #region Side bar item

                SideBarItem = menus.Where(m => m.IsSideBar).Select(m => new
                {
                    Id = m.Id,
                    Title = m.Title,
                    Link = m.Link,
                    OrderId = m.OrderId
                }).OrderBy(m => m.OrderId).ToList()

                #endregion

                #endregion
            }, JsonRequestBehavior.AllowGet);
        }
    }
}