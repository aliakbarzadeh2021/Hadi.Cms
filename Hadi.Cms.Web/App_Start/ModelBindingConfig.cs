using Hadi.Cms.Web.Binders;
using System.Web.Mvc;

namespace Hadi.Cms.Web.App_Start
{
    public class ModelBindingConfig
    {
        public static void RegisterModelBinders(ModelBinderDictionary binders)
        {
            DbGeographyModelBinder.RegisterBinder(binders);
            DateTimeBinder.RegisterBinder(binders);
            KendoRequestParametersBinder.RegisterBinder(binders);
        }
    }
}