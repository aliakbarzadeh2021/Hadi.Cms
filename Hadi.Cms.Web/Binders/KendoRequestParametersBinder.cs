using Hadi.Cms.Infrastructure.Utilities.KendoRequestParameters;
using System.Web.Mvc;

namespace Hadi.Cms.Web.Binders
{
    public class KendoRequestParametersBinder : IModelBinder
    {
        public static void RegisterBinder(ModelBinderDictionary binders)
        {
            if (!binders.ContainsKey(typeof(KendoRequestParameters)))
                binders.Add(typeof(KendoRequestParameters), new KendoRequestParametersBinder());
        }

        object IModelBinder.BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            return KendoRequestParameters.Current;
        }
    }
}