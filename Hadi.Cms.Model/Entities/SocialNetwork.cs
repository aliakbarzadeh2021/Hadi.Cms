using System;
using System.Web.Mvc;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// شبکه های اجتماعی
    /// </summary>
    public class SocialNetwork : BaseModel
    {
        public SocialNetwork()
        {

        }
        public string Title { get; set; }
        public string Link { get; set; }
        [AllowHtml]
        public string Source { get; set; }
        public Guid? ImageGuid { get; set; }
    }
}
