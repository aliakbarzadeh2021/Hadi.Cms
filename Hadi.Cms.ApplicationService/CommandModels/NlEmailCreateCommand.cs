using Hadi.Cms.Language.Resources;
using System.ComponentModel.DataAnnotations;

namespace Hadi.Cms.ApplicationService.CommandModels
{
    /// <summary>
    /// فرمان ثبت ایمیل برای خبرنامه
    /// </summary>
    public class NlEmailCreateCommand
    {
        public string Email { get; set; }
    }
}
