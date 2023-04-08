using System;
using System.ComponentModel.DataAnnotations;
using Hadi.Cms.Language.Resources;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ویرایش تکنولوژی
    /// </summary>
    public class TechnologyEditCommand
    {
        public Guid Id { get; set; }
        [Required(AllowEmptyStrings = false , ErrorMessageResourceType = typeof(Strings),ErrorMessageResourceName = "Required")]
        public string Name { get; set; }
        public Guid? ImageGuid { get; set; }
        public string ImageSource { get; set; }
        public bool IsTool { get; set; }
        public bool IsActive { get; set; }
    }
}
