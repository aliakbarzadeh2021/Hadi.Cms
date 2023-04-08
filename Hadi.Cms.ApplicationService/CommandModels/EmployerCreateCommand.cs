using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ثبت کارفرمای جدید
    /// </summary>
    public class EmployerCreateCommand
    {
        public Guid LogoGuid { get; set; }
        [Required(AllowEmptyStrings = false , ErrorMessageResourceType = typeof(Strings),ErrorMessageResourceName = "Required")]
        public string Name { get; set; }
        public string CEOName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
    }
}
