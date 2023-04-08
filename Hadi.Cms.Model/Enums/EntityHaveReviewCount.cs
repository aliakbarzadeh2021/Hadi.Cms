using System.ComponentModel.DataAnnotations;

namespace Hadi.Cms.Model.Enums
{
    public enum EntityHaveReviewCount
    {
        [Display(Name = "مقالات")]
        Article = 1,
        [Display(Name = "خدمات")]
        Service = 2
    }
}
