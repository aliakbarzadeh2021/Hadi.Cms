using System;
//using System.Data.Spatial;
using System.Globalization;
using System.Web.Mvc;

namespace Hadi.Cms.Web.Binders
{
    public class DbGeographyModelBinder : DefaultModelBinder
    {
        private CultureInfo s_englishUsCulture = new CultureInfo("en-us");

        public static void RegisterBinder(ModelBinderDictionary binders)
        {
            //binders.Add(typeof(DbGeography), new DbGeographyModelBinder());
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            /*if (bindingContext.ModelType == typeof(DbGeography))
            {
                string modelName = bindingContext.ModelName;

                string lat = bindingContext.ValueProvider.GetValue(modelName + ".Latitude").AttemptedValue;
                string lng = bindingContext.ValueProvider.GetValue(modelName + ".Longitude").AttemptedValue;

                double latAsDouble;
                double lngAsDouble;

                if (!double.TryParse(lat, NumberStyles.Any, s_englishUsCulture, out latAsDouble) || !double.TryParse(lng, NumberStyles.Any, s_englishUsCulture, out lngAsDouble))
                    return null;

                DbGeography dbGeography = DbGeography.FromText(String.Format(s_englishUsCulture, "POINT({0} {1})", lngAsDouble, latAsDouble));

                return dbGeography;
            }

            return base.BindModel(controllerContext, bindingContext);*/
            return null;
        }
    }
}