using System.Collections.Generic;

namespace Hadi.Cms.ApplicationService.QueryModels
{
    /// <summary>
    /// دریافت ایتم های مگا منو
    /// </summary>
    public class MegaMenuDto
    {
        public MegaMenuDto()
        {
            MenuItems = new List<MenuDto>();
            SideBarItems = new List<MenuDto>();
        }
        /// <summary>
        /// لیست ایتم های دارای دسته بندی
        /// </summary>
        public List<MenuDto> MenuItems { get; set; }
        /// <summary>
        /// لیست اتم های متفرقه و بدون دسته بندی
        /// </summary>
        public List<MenuDto> SideBarItems { get; set; }
    }
}
