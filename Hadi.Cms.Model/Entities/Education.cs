
using System;
using System.ComponentModel.DataAnnotations;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// تحصیلات
    /// </summary>
    public class Education
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
    }
}
