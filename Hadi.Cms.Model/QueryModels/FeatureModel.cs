using Hadi.Cms.Language.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace Hadi.Cms.Model.QueryModels
{
    public class FeatureModel
    {
        public Guid Id { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "FeatureModel_AreaName")]
        public string AreaName { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "FeatureModel_ControllerName")]
        public string ControllerName { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "FeatureModel_ActionName")]
        public string ActionName { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "FeatureModel_Attributes")]
        public string Attributes { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "FeatureModel_FeaturesName")]
        public string FeaturesName { get; set; }

    }
}
