using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hadi.Cms.Model.Mappings.Interfaces
{
    public interface IFormFieldDto
    {
        /// <summary>
        /// شناسه
        /// </summary>
        Guid Id { get; set; }
        /// <summary>
        /// شناسه فرم
        /// </summary>
        Guid FormId { get; set; }
        /// <summary>
        /// لیبل اینپوت
        /// </summary>
        string Label { get; set; }
        /// <summary>
        /// نام فیلد
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// نوع فیلد
        /// </summary>
        string Type { get; set; }
        IFormDto FormDto { get; set; }
    }
}
