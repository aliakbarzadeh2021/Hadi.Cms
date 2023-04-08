using System;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// فیلد فرم
    /// </summary>
    public class FormField : BaseModel
    {
        public FormField()
        {
            
        }
        /// <summary>
        /// شناسه فرم
        /// </summary>
        public Guid FormId { get; set; }
        /// <summary>
        /// لیبل اینپوت
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// نام فیلد
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// نوع فیلد
        /// </summary>
        public string Type { get; set; }
        public Form Form { get; set; }
    }
}
