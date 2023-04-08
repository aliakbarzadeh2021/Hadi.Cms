using System;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ویرایش دسته بندی
    /// </summary>
    public class NewsCategoryEditCommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string EnTitle { get; set; }
        public int OrderId { get; set; }
    }
}
