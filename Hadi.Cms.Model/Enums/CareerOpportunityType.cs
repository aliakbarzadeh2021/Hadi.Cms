using System.ComponentModel.DataAnnotations;

namespace Hadi.Cms.Model.Enums
{
    public enum CareerOpportunityType
    {
        [Display(Name = "استخدامی")]
        Employment = 0,
        [Display(Name = "کارآموزی")]
        Internship = 1
    }
}
