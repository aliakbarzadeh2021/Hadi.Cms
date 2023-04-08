using System;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// همکاران
    /// </summary>
    public class Partner : BaseModel
    {
        public Partner()
        {
            
        }

        /// <summary>
        /// نام
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// تول تیپ
        /// </summary>
        public string ToolTip { get; set; }
        /// <summary>
        /// لینک
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// شناسه عکس
        /// </summary>
        public Guid? AttachmentImageGuid { get; set; }
    }
}
