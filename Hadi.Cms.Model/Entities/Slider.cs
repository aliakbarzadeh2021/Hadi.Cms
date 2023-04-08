using System.Collections.Generic;

namespace Hadi.Cms.Model.Entities
{
    /// <summary>
    /// اسلایدر
    /// </summary>
    public class Slider : BaseModel
    {
        public Slider()
        {
            SliderItems = new HashSet<SliderItem>();
        }
        /// <summary>
        /// عنوان اسلایدر
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// توضیحات اسلایدر
        /// </summary>
        public string Description { get; set; }

        public ICollection<SliderItem> SliderItems { get; set; }
    }
}
