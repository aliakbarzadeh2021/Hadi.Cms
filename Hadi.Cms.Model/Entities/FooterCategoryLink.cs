using System;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// لینک های دسته بندیی های فوتر
    /// </summary>
    public class FooterCategoryLink : BaseModel
    {
        public FooterCategoryLink()
        {
            
        }

        /// <summary>
        /// شناسه دسته بندی فوتر
        /// </summary>
        public Guid FooterCategoryId { get; set; }
        /// <summary>
        /// لینک
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// متن لینک
        /// </summary>
        public string LinkText { get; set; }
        /// <summary>
        /// شناسه عکس
        /// </summary>
        public Guid? ImageAttachmentGuid { get; set; }

        public FooterCategory FooterCategory { get; set; }
    }
}
